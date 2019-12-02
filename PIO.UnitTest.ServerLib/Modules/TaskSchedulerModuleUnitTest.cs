using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.ServerLib;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class TaskSchedulerModuleUnitTest
	{
		[TestMethod]
		public void ShouldInitialize()
		{
			TaskSchedulerModule module;
			bool result;

			module = new TaskSchedulerModule(NullLogger.Instance, new MockedTaskModule(false,3));
			result = module.Initialize();
			Assert.IsTrue(result);
			Assert.AreEqual(3, module.Count);
		}
		
		[TestMethod]
		public void ShouldNotInitializeAndLogError()
		{
			TaskSchedulerModule module;
			MemoryLogger logger;
			bool result;


			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new TaskSchedulerModule(logger, new MockedTaskModule(true,3));
			result = module.Initialize();
			Assert.IsFalse(result);
			Assert.AreEqual(0, module.Count);
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldRegisterTaskHandler()
		{
			TaskSchedulerModule module;

			module = new TaskSchedulerModule(NullLogger.Instance, new MockedTaskModule(false, 3));
			Assert.IsTrue(module.Register(new MockedTaskHandler(0)));
			Assert.IsTrue(module.Register(new MockedTaskHandler(1)));
			Assert.IsTrue(module.Register(new MockedTaskHandler(2)));

		}

		[TestMethod]
		public void ShouldNotRegisterTaskHandlerAndLogError()
		{
			TaskSchedulerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new TaskSchedulerModule(logger, new MockedTaskModule(true, 3));
			Assert.IsTrue(module.Register(new MockedTaskHandler(0)));
			Assert.IsFalse(module.Register(new MockedTaskHandler(0)));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}

	}
}
