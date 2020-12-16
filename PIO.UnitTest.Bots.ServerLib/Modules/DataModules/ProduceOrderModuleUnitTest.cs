using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Bots.Models;
using PIO.Bots.ServerLib.Modules;
using PIO.ModulesLib.Exceptions;
using PIO.UnitTest.Bots.ServerLib.Mocks;

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class ProduceOrderModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetProduceOrder()
		{
			MockedDatabase<ProduceOrder> database;
			ProduceOrderModule module;
			ProduceOrder result;

			database = new MockedDatabase<ProduceOrder>(false, 1, (t) => new ProduceOrder() { ProduceOrderID = t,OrderID=1,FactoryID=2 });
			module = new ProduceOrderModule(NullLogger.Instance, database);
			result = module.GetProduceOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.ProduceOrderID);
			Assert.AreEqual(1, result.OrderID);
			Assert.AreEqual(2, result.FactoryID);
		}

		[TestMethod]
		public void ShouldGetAllProduceOrders()
		{
			MockedDatabase<ProduceOrder> database;
			ProduceOrderModule module;
			ProduceOrder[] results;

			database = new MockedDatabase<ProduceOrder>(false, 3, (t) => new ProduceOrder() { ProduceOrderID = t, OrderID = 1, FactoryID = 2 });
			module = new ProduceOrderModule(NullLogger.Instance, database);
			results = module.GetProduceOrders();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].ProduceOrderID);
				Assert.AreEqual(1, results[t].OrderID);
				Assert.AreEqual(2, results[t].FactoryID);
			}
		}
		
		[TestMethod]
		public void ShouldNotGetProduceOrderAndLogError()
		{
			MockedDatabase<ProduceOrder> database;
			ProduceOrderModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<ProduceOrder>(true,1, (t) => new ProduceOrder() { ProduceOrderID = t, OrderID = 1, FactoryID = 2 });
			module = new ProduceOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		
		[TestMethod]
		public void ShouldNotGetAllProduceOrdersAndLogError()
		{
			MockedDatabase<ProduceOrder> database;
			ProduceOrderModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<ProduceOrder>(true, 3, (t) => new ProduceOrder() { ProduceOrderID = t, OrderID = 1, FactoryID = 2 });
			module = new ProduceOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetProduceOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		





	}
}
