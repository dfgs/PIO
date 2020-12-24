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
	public class CarrierModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldCarryTo()
		{
			CarrierModule module;
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
			module = new CarrierModule(NullLogger.Instance, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			result = module.BeginCarryTo(1,2, ResourceTypeIDs.Wood);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}

		[TestMethod]
		public void ShouldNotCarryWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			CarrierModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID = 1 });

			logger = new MemoryLogger();
			module = new CarrierModule(logger, taskModule, workerModule,null,null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginCarryTo(1, 1,ResourceTypeIDs.Coal));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotEndTaskWhenTargetFactoryDoesntExists()
		{
			MemoryLogger logger;
			CarrierModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}

		[TestMethod]
		public void ShouldNotCarryToWhenFactoryHasNotEnoughResourcesToCarryTo()
		{
			MemoryLogger logger;
			CarrierModule module;
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
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginCarryTo(1,2, ResourceTypeIDs.Stone));
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
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginCarryTo(1,2, ResourceTypeIDs.Stone));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

		}

	
		[TestMethod]
		public void ShouldNotCarryToWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			CarrierModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCarryTo(2,2,ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			CarrierModule module;
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
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCarryTo(1,2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger();
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}



		[TestMethod]
		public void ShouldEndTaskWhenStackExists()
		{
			CarrierModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			module = new CarrierModule(NullLogger.Instance, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndCarryTo(1,2,ResourceTypeIDs.Stone);
			stack = stackModule.GetStack(2);
			Assert.AreEqual(11, stack.Quantity);
			//Assert.AreEqual(2, workerModule.GetWorker(1).FactoryID);

		}
		[TestMethod]
		public void ShouldEndTaskWhenStackDoesntExists()
		{
			CarrierModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			buildingModule = new MockedBuildingModule(false, new Building() {BuildingID=2, X = 10, Y = 10 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1, X = 10, Y = 10 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			module = new CarrierModule(NullLogger.Instance, taskModule, workerModule, buildingModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndCarryTo(1,2,ResourceTypeIDs.Tree);
			stack = stackModule.GetStack(4);
			Assert.AreEqual(1, stack.Quantity);
			//Assert.AreEqual(2, workerModule.GetWorker(1).FactoryID);

		}





		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			CarrierModule module;
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
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndCarryTo(2,2,ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			CarrierModule module;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 2, X = 10, Y = 10 });
			
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(true);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCarryTo(1,2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));


			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new CarrierModule(logger, taskModule, workerModule, buildingModule,  stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));



		}




	}
}
