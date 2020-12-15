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

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(NullLogger.Instance, taskModule, workerModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginIdle(1, 10);

			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.Idle, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldNotIdleWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID=1}) ;

			logger = new MemoryLogger();
			module = new IdlerModule(logger, taskModule,  workerModule);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginIdle(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}




		[TestMethod]
		public void ShouldNotIdleWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new IdlerModule(logger, taskModule, workerModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginIdle(2, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger();
			module = new IdlerModule(logger, taskModule,  workerModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginIdle(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));


			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new IdlerModule(logger, taskModule,  workerModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginIdle(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

		}



		[TestMethod]
		public void ShouldEndTasks()
		{
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
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

			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(logger, taskModule,  workerModule);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginIdle(2, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}
		
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			IdlerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

		
			
			logger = new MemoryLogger();
			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false);
			module = new IdlerModule(logger, taskModule, workerModule);

			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginIdle(1, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));


		}






	}
}
