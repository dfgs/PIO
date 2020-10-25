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
	public class IdlerModuleUnitTest
	{

		[TestMethod]
		public void ShouldIdle()
		{
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(NullLogger.Instance, taskModule, workerModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginIdle(1, 10);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldEnqueueTaskWithCorrectETA()
		{
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result,result2;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(NullLogger.Instance, taskModule,  workerModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginIdle(1, 10);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
			
			result2 = module.BeginIdle(1, 10);
			Assert.IsNotNull(result2);
			Assert.AreEqual(1, result2.WorkerID);
			Assert.AreEqual(2, schedulerModule.Count);

			Assert.IsTrue((result2.ETA - result.ETA).TotalSeconds >= 4);
		}



		
		[TestMethod]
		public void ShouldNotIdleWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new IdlerModule(logger, taskModule, workerModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginIdle(2, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new IdlerModule(logger, taskModule,  workerModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginIdle(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));


			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new IdlerModule(logger, taskModule,  workerModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginIdle(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

		}



		[TestMethod]
		public void ShouldEndTasks()
		{
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(NullLogger.Instance, taskModule,  workerModule);

			module.EndIdle(1);
			// nothing to check
		}
		
		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			logger = new MemoryLogger(new DefaultLogFormatter());
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(logger, taskModule,  workerModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginIdle(2, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}
		
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

		
			
			logger = new MemoryLogger(new DefaultLogFormatter());
			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, FactoryID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(logger, taskModule, workerModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginIdle(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));


		}






	}
}
