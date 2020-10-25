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
	public class MoverModuleUnitTest
	{

		[TestMethod]
		public void ShouldMove()
		{
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(NullLogger.Instance, taskModule, workerModule, factoryModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginMoveTo(1, 2);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldEnqueueTaskWithCorrectETA()
		{
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result,result2;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(NullLogger.Instance, taskModule, workerModule, factoryModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginMoveTo(1, 2);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
			
			result2 = module.BeginMoveTo(1, 2);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(2, schedulerModule.Count);

			Assert.IsTrue((result2.ETA - result.ETA).TotalSeconds >= 4);
		}



		[TestMethod]
		public void ShouldNotMoveWhenFactoryDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(1, 3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotMoveWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

			factoryModule = new MockedFactoryModule(true, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

		}



		[TestMethod]
		public void ShouldEndTasks()
		{
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(NullLogger.Instance, taskModule, workerModule, factoryModule);

			module.EndMoveTo(1, 2);
			Assert.AreEqual(2, workerModule.GetWorker(1).FactoryID);
		}
		
		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			logger = new MemoryLogger(new DefaultLogFormatter());
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndTaskWhenTargetFactoryDoesntExists()
		{
			MemoryLogger logger;
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			logger = new MemoryLogger(new DefaultLogFormatter());
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginMoveTo(1, 3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			MoverModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			logger = new MemoryLogger(new DefaultLogFormatter());
			factoryModule = new MockedFactoryModule(true, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
			
			
			logger = new MemoryLogger(new DefaultLogFormatter());
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3  }, new Factory() { FactoryID = 2, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3,  });
			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new MoverModule(logger, taskModule, workerModule, factoryModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginMoveTo(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));


		}






	}
}
