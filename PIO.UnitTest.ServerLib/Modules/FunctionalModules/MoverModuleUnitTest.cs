using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;
using PIO.UnitTest.ServerLib.Mocks.EngineModules;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class MoverModuleUnitTest
	{

		[TestMethod]
		public void ShouldMove()
		{
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(NullLogger.Instance, taskModule, workerModule,  buildingModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginMoveTo(1, 2,2);

			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldNotMoveWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			MoverModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID = 1 });

			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule,null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginMoveTo(1, 1,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}




		[TestMethod]
		public void ShouldNotMoveWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule, buildingModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(2, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotMoveAndThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule,  buildingModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule,  buildingModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

		}









		[TestMethod]
		public void ShouldMoveToBuilding()
		{
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 1, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(NullLogger.Instance, taskModule, workerModule, buildingModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginMoveTo(1, 3);

			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldNotMoveToBuildingWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			MoverModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID = 1 });

			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule,  null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginMoveTo(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}




		[TestMethod]
		public void ShouldNotMoveToBuildingWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule,  buildingModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(2, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotMoveToBuildingWhenFactoryDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule,  buildingModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotMoveToBuildingWhenBuildingDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule,  buildingModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}


		[TestMethod]
		public void ShouldNotMoveToBuildingWheWorkerIsNotOnSamePlanet()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, PlanetID=2, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new MoverModule(logger, taskModule, workerModule, buildingModule);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginMoveTo(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}




		[TestMethod]
		public void ShouldEndTasks()
		{
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(NullLogger.Instance, taskModule, workerModule,  buildingModule);

			module.EndMoveTo(1, 2, 2);
			Assert.AreEqual(2, workerModule.GetWorker(1).X);
			Assert.AreEqual(2, workerModule.GetWorker(1).Y);
		}

		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			logger = new MemoryLogger();
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(logger, taskModule, workerModule,  buildingModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(2, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}
		
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			MoverModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			logger = new MemoryLogger();
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(true);
			module = new MoverModule(logger, taskModule, workerModule, buildingModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			
			
			logger = new MemoryLogger();
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, X = 1, Y = 1 });
			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(logger, taskModule, workerModule, buildingModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));


		}






	}
}
