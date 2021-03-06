﻿using System;
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
	public class TakerModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldTake()
		{
			TakerModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			buildingModule = new MockedBuildingModule(false, new Building() {BuildingID=2, X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			module = new TakerModule(NullLogger.Instance, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			result = module.BeginTake(1, ResourceTypeIDs.Wood);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}

		[TestMethod]
		public void ShouldNotTakeWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			TakerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID = 1 });

			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule,null,null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginTake(1, ResourceTypeIDs.Coal));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotTakeWhenWorkerIsAlreadyCarryingItem()
		{
			MemoryLogger logger;
			TakerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 ,ResourceTypeID=ResourceTypeIDs.Wood});
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, null, null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginTake(1, ResourceTypeIDs.Coal));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotTakeWhenFactoryHasNotEnoughResourcesToTake()
		{
			MemoryLogger logger;
			TakerModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() {BuildingID=2, X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 0 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginTake(1, ResourceTypeIDs.Stone));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginTake(1, ResourceTypeIDs.Stone));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

		}

	
		[TestMethod]
		public void ShouldNotTakeWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			TakerModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { X = 10, Y = 10,BuildingID=2 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginTake(2,ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}

	

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			TakerModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			buildingModule = new MockedBuildingModule(false, new Building() {BuildingID=2, X = 10, Y = 10 });

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(true, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginTake(1, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginTake(1,  ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginTake(1,  ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}



		[TestMethod]
		public void ShouldEndTask()
		{
			TakerModule module;
			IBuildingModule buildingModule;
			MockedWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			module = new TakerModule(NullLogger.Instance, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndTake(1,ResourceTypeIDs.Stone);
			Assert.AreEqual(ResourceTypeIDs.Stone, workerModule.ResourceTypeID);
			//Assert.AreEqual(2, workerModule.GetWorker(1).FactoryID);

		}
	





		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			TakerModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndTake(2,ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			TakerModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, X = 10, Y = 10 });
			

			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new TakerModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndTake(1, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));



		}




	}
}
