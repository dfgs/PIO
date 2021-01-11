using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using NetORMLib.Queries;
using NSubstitute;
using PIO.Bots.Models;
using PIO.Bots.ServerLib.Modules;
using PIO.ModulesLib.Exceptions;

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class OrderModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetOrder()
		{
			OrderModule module;
			Order result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<Order>(Arg.Any<ISelect>()).Returns(new Order[] { new Order() { OrderID = 1 } });

			module = new OrderModule(NullLogger.Instance, database);
			result = module.GetOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.OrderID);
		}
		
		[TestMethod]
		public void ShouldGetAllOrders()
		{
			OrderModule module;
			Order[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<Order>(Arg.Any<ISelect>()).Returns(new Order[] { new Order() { OrderID = 1 }, new Order() { OrderID = 2 }, new Order() { OrderID = 3 } });

			module = new OrderModule(NullLogger.Instance, database);
			results = module.GetOrders();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t+1, results[t].OrderID);
			}
		}

		[TestMethod]
		public void ShouldNotGetOrderAndLogError()
		{
			OrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<Order>(Arg.Any<ISelect>()).Returns((x)=> { throw new Exception(); });

			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		
		[TestMethod]
		public void ShouldNotGetAllOrdersAndLogError()
		{
			OrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<Order>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}



		[TestMethod]
		public void ShouldCreateOrder()
		{
			OrderModule module;
			Order result;
			IDatabase database;
			int inserted = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any < IQuery[]>() )).Do(x=>inserted++);

			module = new OrderModule(NullLogger.Instance, database);

			result = module.CreateOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.PlanetID);
			Assert.AreEqual(1, inserted);
		}

		[TestMethod]
		public void ShouldNotCreateOrderAndLogError()
		{
			OrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => { throw new Exception(); });

			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldAssignOrder()
		{
			OrderModule module;
			IDatabase database;
			int updated = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IUpdate>())).Do(x => updated++);

			module = new OrderModule(NullLogger.Instance, database);
			module.Assign(1, 1);
			Assert.AreEqual(1, updated);
		}
		[TestMethod]
		public void ShouldUnAssignAll()
		{
			OrderModule module;
			IDatabase database;
			int updated = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IUpdate>())).Do(x => updated++);

			module = new OrderModule(NullLogger.Instance, database);
			module.UnAssignAll(1);
			Assert.AreEqual(1, updated);
		}
		[TestMethod]
		public void ShouldNotAssignOrder()
		{
			OrderModule module;
			IDatabase database;
			MemoryLogger logger;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IUpdate>())).Do(x => { throw new Exception(); });

			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.Assign(1,1));
		}
		[TestMethod]
		public void ShouldNotUnAssignAll()
		{
			OrderModule module;
			IDatabase database;
			MemoryLogger logger;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IUpdate>())).Do(x => { throw new Exception(); });

			module = new OrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.UnAssignAll(1));
		}




	}
}
