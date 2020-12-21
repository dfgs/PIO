using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Bots.Models;
using PIO.Bots.ServerLib.Modules;
using PIO.Models;
using PIO.ModulesLib.Exceptions;
using PIO.UnitTest.Bots.ServiceLib.Mocks;

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class OrderManagerModuleUnitTest
	{
		[TestMethod]
		public void ShouldCreateProduceOrder()
		{
			MockedOrderModule orderModule;
			MockedProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			ProduceOrder result;

			orderModule = new MockedOrderModule(0, false);
			produceOrderModule = new MockedProduceOrderModule(0, false);
			module = new OrderManagerModule(NullLogger.Instance, orderModule,produceOrderModule);
			result = module.CreateProduceOrder(1);
			Assert.AreEqual(1, module.ProduceOrderCount);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);

			result = module.CreateProduceOrder(1);
			Assert.AreEqual(2, module.ProduceOrderCount);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);

		}



		[TestMethod]
		public void ShouldNotCreateProduceOrder()
		{
			MockedOrderModule orderModule;
			MockedProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			orderModule = new MockedOrderModule(0, true);
			produceOrderModule = new MockedProduceOrderModule(0, false);
			module = new OrderManagerModule(logger, orderModule, produceOrderModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, module.ProduceOrderCount);


			logger = new MemoryLogger();
			orderModule = new MockedOrderModule(0, false);
			produceOrderModule = new MockedProduceOrderModule(0, true);
			module = new OrderManagerModule(logger, orderModule, produceOrderModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, module.ProduceOrderCount);
		}




		[TestMethod]
		public void ShouldReturnIdleTaskIfNoOrderArePresent()
		{
			MockedOrderModule orderModule;
			MockedProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			Task result;

			orderModule = new MockedOrderModule(0, false);
			produceOrderModule = new MockedProduceOrderModule(0, false);
			module = new OrderManagerModule(NullLogger.Instance, orderModule, produceOrderModule);
			result = module.CreateTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Idle, result.TaskTypeID);

		}




	}
}
