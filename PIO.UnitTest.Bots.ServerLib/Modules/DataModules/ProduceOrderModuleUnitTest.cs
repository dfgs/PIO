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
	public class ProduceProduceOrderModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetProduceOrder()
		{
			ProduceOrderModule module;
			ProduceOrder result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns(new ProduceOrder[] { new ProduceOrder() { ProduceOrderID = 1 } });

			module = new ProduceOrderModule(NullLogger.Instance, database);
			result = module.GetProduceOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ProduceOrderID);
		}

		[TestMethod]
		public void ShouldGetAllProduceOrders()
		{
			ProduceOrderModule module;
			ProduceOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns(new ProduceOrder[] { new ProduceOrder() { ProduceOrderID = 1 }, new ProduceOrder() { ProduceOrderID = 2 }, new ProduceOrder() { ProduceOrderID = 3 } });

			module = new ProduceOrderModule(NullLogger.Instance, database);
			results = module.GetProduceOrders();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].ProduceOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetProduceOrdersForPlanet()
		{
			ProduceOrderModule module;
			ProduceOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns(new ProduceOrder[] { new ProduceOrder() { ProduceOrderID = 1 }, new ProduceOrder() { ProduceOrderID = 2 }, new ProduceOrder() { ProduceOrderID = 3 } });

			module = new ProduceOrderModule(NullLogger.Instance, database);
			results = module.GetProduceOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].ProduceOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetWaitingProduceOrdersForPlanet()
		{
			ProduceOrderModule module;
			ProduceOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns(new ProduceOrder[] { new ProduceOrder() { ProduceOrderID = 1 }, new ProduceOrder() { ProduceOrderID = 2 }, new ProduceOrder() { ProduceOrderID = 3 } });

			module = new ProduceOrderModule(NullLogger.Instance, database);
			results = module.GetWaitingProduceOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].ProduceOrderID);
			}
		}

		[TestMethod]
		public void ShouldNotGetProduceOrderAndLogError()
		{
			ProduceOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new ProduceOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetAllProduceOrdersAndLogError()
		{
			ProduceOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new ProduceOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetProduceOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetProduceOrdersForPlanetAndLogError()
		{
			ProduceOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new ProduceOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetProduceOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWaitingProduceOrdersForPlanetAndLogError()
		{
			ProduceOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<ProduceOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new ProduceOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWaitingProduceOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreateProduceOrder()
		{
			ProduceOrderModule module;
			ProduceOrder result;
			IDatabase database;
			int inserted = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => inserted++);

			module = new ProduceOrderModule(NullLogger.Instance, database);

			result = module.CreateProduceOrder(1,2);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, inserted);
		}

		[TestMethod]
		public void ShouldNotCreateProduceOrderAndLogError()
		{
			ProduceOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => { throw new Exception(); });

			module = new ProduceOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateProduceOrder(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}



	}
}
