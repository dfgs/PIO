using System;
using System.Linq;
using System.ServiceModel;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using PIO.UnitTest.WebServiceLib.Mocks;
using PIO.WebServiceLib;

namespace PIO.UnitTest.WebServiceLib
{
	[TestClass]
	public class TaskCallbackServiceUnitTest
	{

		[TestMethod]
		public void ShouldSubscribe()
		{
			MemoryLogger logger;
			TaskCallbackService  module;
			ITaskCallBack callback;

			logger = new MemoryLogger();

			callback = new MockedCallback(false);
			module = new TaskCallbackService(logger, new MockedSchedulerModule(false));
			Assert.IsTrue(module.Subscribe(callback));
			Assert.IsNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.IsFalse(module.Subscribe(callback));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));

		}
		[TestMethod]
		public void ShouldUnsubscribe()
		{
			MemoryLogger logger;
			TaskCallbackService module;
			ITaskCallBack callback;

			logger = new MemoryLogger();

			callback = new MockedCallback(false);
			module = new TaskCallbackService(logger, new MockedSchedulerModule(false));
			Assert.IsFalse(module.Unsubscribe(callback));
			module.Subscribe(callback);
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.IsTrue(module.Unsubscribe(callback));

		}//

		[TestMethod]
		public void ShouldNotifySubscribers()
		{
			MemoryLogger logger;
			TaskCallbackService module;
			MockedCallback callback;
			MockedSchedulerModule mockedSchedulerModule;

			logger = new MemoryLogger();

			mockedSchedulerModule = new MockedSchedulerModule(false);
			callback = new MockedCallback(false);
			module = new TaskCallbackService(logger, mockedSchedulerModule);
			Assert.IsTrue(module.Subscribe(callback));

			Assert.AreEqual(0, callback.StartedCount);
			mockedSchedulerModule.InvokeTaskStarted();
			Assert.AreEqual(1, callback.StartedCount);

			Assert.AreEqual(0, callback.EndedCount);
			mockedSchedulerModule.InvokeTaskEnded();
			Assert.AreEqual(1, callback.EndedCount);

		}

		[TestMethod]
		public void ShouldNNotNotifySubscribersAfterUnsubscribe()
		{
			MemoryLogger logger;
			TaskCallbackService module;
			MockedCallback callback;
			MockedSchedulerModule mockedSchedulerModule;

			logger = new MemoryLogger();

			mockedSchedulerModule = new MockedSchedulerModule(false);
			callback = new MockedCallback(false);
			module = new TaskCallbackService(logger, mockedSchedulerModule);
			Assert.IsTrue(module.Subscribe(callback));
			Assert.IsTrue(module.Unsubscribe(callback));

			Assert.AreEqual(0, callback.StartedCount);
			mockedSchedulerModule.InvokeTaskStarted();
			Assert.AreEqual(0, callback.StartedCount);

			Assert.AreEqual(0, callback.EndedCount);
			mockedSchedulerModule.InvokeTaskEnded();
			Assert.AreEqual(0, callback.EndedCount);

		}
		[TestMethod]
		public void ShouldNNotNotifySubscribersAndLogError()
		{
			MemoryLogger logger;
			TaskCallbackService module;
			MockedCallback callback;
			MockedSchedulerModule mockedSchedulerModule;

			logger = new MemoryLogger();

			mockedSchedulerModule = new MockedSchedulerModule(false);
			callback = new MockedCallback(true);
			module = new TaskCallbackService(logger, mockedSchedulerModule);
			Assert.IsTrue(module.Subscribe(callback));

			Assert.AreEqual(0, callback.StartedCount);
			mockedSchedulerModule.InvokeTaskStarted();
			Assert.AreEqual(0, callback.StartedCount);
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));

			Assert.AreEqual(0, callback.EndedCount);
			mockedSchedulerModule.InvokeTaskEnded();
			Assert.AreEqual(0, callback.EndedCount);
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));

		}

	}
}
