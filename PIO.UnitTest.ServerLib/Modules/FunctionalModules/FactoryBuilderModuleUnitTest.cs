using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;
using PIO.UnitTest.ServerLib.Mocks.EngineModules;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class FactoryBuilderModuleUnitTest
	{

		[TestMethod]
		public void ShouldCreateBuilding()
		{
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			Task result;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false,new FactoryType() { FactoryTypeID=FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(NullLogger.Instance, taskModule, workerModule, buildingModule,factoryModule,factoryTypeModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginCreateBuilding(1,FactoryTypeIDs.Sawmill) ;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(FactoryTypeIDs.Sawmill, result.FactoryTypeID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldEnqueueTaskWithCorrectETA()
		{
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			
			Task result, result2;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginCreateBuilding(1, FactoryTypeIDs.Sawmill);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(FactoryTypeIDs.Sawmill, result.FactoryTypeID);
			Assert.AreEqual(1, schedulerModule.Count);

			result2 = module.BeginCreateBuilding(1,  FactoryTypeIDs.Sawmill);
			Assert.IsNotNull(result2);
			Assert.AreEqual(1, result2.WorkerID);
			Assert.AreEqual(FactoryTypeIDs.Sawmill, result2.FactoryTypeID);
			Assert.AreEqual(2, schedulerModule.Count);

			Assert.IsTrue((result2.ETA - result.ETA).TotalSeconds >= 4);
		}


		[TestMethod]
		public void ShouldNotCreateBuildingWhenPositionIsOccupied()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			Task result;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1,X=10,Y=10 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() {BuildingID=1,X=10,Y=10 } );
			factoryModule = new MockedFactoryModule(false, new Factory() { BuildingID=1 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule);

			result = module.BeginCreateBuilding(1, FactoryTypeIDs.Sawmill);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCreateBuilding(999, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
		}


		[TestMethod]
		public void ShouldNotCreateBuildingWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCreateBuilding(999,  FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotCreateBuildingWhenFactoryTypeDoesntExists()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(true);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));


			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCreateBuilding(1,  FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(true, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCreateBuilding(1,  FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

		}



		[TestMethod]
		public void ShouldEndTasks()
		{
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule);

			module.EndCreateBuilding(1,FactoryTypeIDs.Sawmill);
			// nothing to check
		}

		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;

			logger = new MemoryLogger(new DefaultLogFormatter());
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndCreateBuilding(999, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}

		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			;



			logger = new MemoryLogger(new DefaultLogFormatter());
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(true, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));



			logger = new MemoryLogger(new DefaultLogFormatter());
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(true);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

		}

	}
}
