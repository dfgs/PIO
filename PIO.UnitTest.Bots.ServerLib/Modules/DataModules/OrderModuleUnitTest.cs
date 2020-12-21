using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Bots.Models;
using PIO.Bots.ServerLib.Modules;
using PIO.ModulesLib.Exceptions;
using PIO.UnitTest.Bots.ServiceLib.Mocks;

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class OrderModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetOrder()
		{
			MockedDatabase<Order> database;
			OrderModule module;
			Order result;

			database = new MockedDatabase<Order>(false, 1, (t) => new Order() { OrderID = t });
			module = new OrderModule(NullLogger.Instance, database);
			result = module.GetOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.OrderID);
		}
		
		[TestMethod]
		public void ShouldGetAllOrders()
		{
			MockedDatabase<Order> database;
			OrderModule module;
			Order[] results;

			database = new MockedDatabase<Order>(false, 3, (t) => new Order() { OrderID = t });
			module = new OrderModule(NullLogger.Instance, database);
			results = module.GetOrders();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].OrderID);
			}
		}
		
		[TestMethod]
		public void ShouldNotGetOrderAndLogError()
		{
			MockedDatabase<Order> database;
			OrderModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Order>(true,1, (t) => new Order() { OrderID = t });
			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		
		[TestMethod]
		public void ShouldNotGetAllOrdersAndLogError()
		{
			MockedDatabase<Order> database;
			OrderModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Order>(true, 3, (t) => new Order() { OrderID = t });
			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}



		[TestMethod]
		public void ShouldCreateOrder()
		{
			MockedDatabase<Order> database;
			OrderModule module;
			Order result;

			database = new MockedDatabase<Order>(false, 1, (t) => new Order() { OrderID = t });
			module = new OrderModule(NullLogger.Instance, database);

			result = module.CreateOrder();
			Assert.IsNotNull(result);
			Assert.AreEqual(1, database.InsertedCount);
		}

		[TestMethod]
		public void ShouldNotCreateOrderAndLogError()
		{
			MockedDatabase<Order> database;
			OrderModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Order>(true, 1, (t) => new Order() { OrderID = t });
			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateOrder());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}




	}
}
