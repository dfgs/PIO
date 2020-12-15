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
			IStackModule stackModule;
			IMaterialModule materialModule;
			Task result;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false,new FactoryType() { FactoryTypeID=FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(NullLogger.Instance, taskModule, workerModule, buildingModule,factoryModule,factoryTypeModule, stackModule, materialModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginCreateBuilding(1,FactoryTypeIDs.Sawmill) ;

			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.CreateBuilding, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(FactoryTypeIDs.Sawmill, result.FactoryTypeID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldNotCreateBuildingWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID = 1 });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule,null,null,null,null,null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginCreateBuilding(1, FactoryTypeIDs.Forest));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
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
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1,X=10,Y=10 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() {BuildingID=1,X=10,Y=10 } );
			factoryModule = new MockedFactoryModule(false, new Factory() { BuildingID=1 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
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
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;
			

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCreateBuilding(999,  FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
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
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;
			

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false);
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotCreateBuildingAndThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;
			

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(true);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));


			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCreateBuilding(1,  FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(true, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCreateBuilding(1,  FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

		}



		[TestMethod]
		public void ShouldEndCreateBuildingTasks()
		{
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedBuildingModule buildingModule;
			MockedFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;
			

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			module.EndCreateBuilding(1,FactoryTypeIDs.Sawmill);
			Assert.AreEqual(buildingModule.Count, 1);
			Assert.AreEqual(factoryModule.Count, 1);
		}

		[TestMethod]
		public void ShouldNotEndCreateBuildingTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;

			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndCreateBuilding(999, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}

		[TestMethod]
		public void ShouldNotEndCreateBuildingTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;
			



			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(true, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));



			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(true);
			factoryModule = new MockedFactoryModule(false);
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule,  buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCreateBuilding(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

		}











		[TestMethod]
		public void ShouldBuild()
		{
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			Task result;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false,new Building() { BuildingID=2, RemainingBuildSteps=5});
			factoryModule = new MockedFactoryModule(false,new Factory() {FactoryID=2,BuildingID=2, FactoryTypeID=FactoryTypeIDs.Sawmill }) ;
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID=2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginBuild(1);

			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.Build, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldNotBuildWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID = 1 });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, null, null, null, null, null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}


		[TestMethod]
		public void ShouldNotBuildWhenNoResourceAvailable()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;


			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 0, Y = 0 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 0, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			//result = module.BeginBuild(1, 2);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotBuildWhenRemainingBuildStepsIsZero()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 0 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotBuildWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginBuild(999));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotBuildWhenFactoryDoesntExistsAtPos()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;

			
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 ,X=10,Y=10});
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotBuildAndThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;

	

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(true);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));


			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(true, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(true, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(true, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(true, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });

			logger = new MemoryLogger();
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

		}





		[TestMethod]
		public void ShouldEndBuildTasks()
		{
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;


			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);
	
			module.EndBuild(1);
			Assert.AreEqual(4, buildingModule.GetBuilding(2).RemainingBuildSteps);
		}

		[TestMethod]
		public void ShouldNotEndBuildTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;


			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndBuild(999));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndBuildTaskWhenFactoryDoesntExistsAtPos()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;


			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1,X=10,Y=10 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndBuildTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			FactoryBuilderModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			IFactoryTypeModule factoryTypeModule;




			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));




			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(true, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));




			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, RemainingBuildSteps = 5 });
			factoryModule = new MockedFactoryModule(true, new Factory() { FactoryID = 2, BuildingID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 }, new Stack() { FactoryID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Stone, StackID = 1 });
			materialModule = new MockedMaterialModule(false, new Material() { Quantity = 1, ResourceTypeID = ResourceTypeIDs.Wood, FactoryTypeID = FactoryTypeIDs.Sawmill }, new Material() { Quantity = 2, ResourceTypeID = ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill });
			module = new FactoryBuilderModule(logger, taskModule, workerModule, buildingModule, factoryModule, factoryTypeModule, stackModule, materialModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

		}




	}
}
