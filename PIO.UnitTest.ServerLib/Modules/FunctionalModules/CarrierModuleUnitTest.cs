using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Modules;
using PIO.Models.Exceptions;
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
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			module = new CarrierModule(NullLogger.Instance, taskModule, factoryModule,workerModule,stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			result = module.BeginCarryTo(1,2, ResourceTypeIDs.Wood);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}

		[TestMethod]
		public void ShouldEnqueueTaskWithCorrectETA()
		{
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result,result2;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 20 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 20 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 20 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 20 }
				);
			taskModule = new MockedTaskModule(false);
			module = new CarrierModule(NullLogger.Instance, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);

			result2 = module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(2, schedulerModule.Count);

			Assert.IsTrue((result2.ETA - result.ETA).TotalSeconds >= 1);


		}

		[TestMethod]
		public void ShouldNotEndTaskWhenTargetFactoryDoesntExists()
		{
			MemoryLogger logger;
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}

		[TestMethod]
		public void ShouldNotCarryToWhenFactoryHasNotEnoughResourcesToCarryTo()
		{
			MemoryLogger logger;
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 0 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule,workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginCarryTo(1,2, ResourceTypeIDs.Stone));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginCarryTo(1,2, ResourceTypeIDs.Stone));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

		}

	
		[TestMethod]
		public void ShouldNotCarryToWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule,workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginCarryTo(2,2,ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(true, new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule,stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCarryTo(1,2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false, new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}



		[TestMethod]
		public void ShouldEndTaskWhenStackExists()
		{
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			module = new CarrierModule(NullLogger.Instance, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndCarryTo(1,2,ResourceTypeIDs.Stone);
			stack = stackModule.GetStack(2);
			Assert.AreEqual(11, stack.Quantity);
			Assert.AreEqual(2, workerModule.GetWorker(1).FactoryID);

		}
		[TestMethod]
		public void ShouldEndTaskWhenStackDoesntExists()
		{
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			taskModule = new MockedTaskModule(false);
			module = new CarrierModule(NullLogger.Instance, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndCarryTo(1,2,ResourceTypeIDs.Tree);
			stack = stackModule.GetStack(4);
			Assert.AreEqual(1, stack.Quantity);
			Assert.AreEqual(2, workerModule.GetWorker(1).FactoryID);

		}





		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false);
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndCarryTo(2,2,ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			CarrierModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(true);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCarryTo(1,2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));


			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, FactoryID = 1 });
			stackModule = new MockedStackModule(false);
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new CarrierModule(logger, taskModule, factoryModule, workerModule, stackModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndCarryTo(1, 2, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));



		}




	}
}
