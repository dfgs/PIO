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
	public class HarvestHarvestOrderModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetHarvestOrder()
		{
			HarvestOrderModule module;
			HarvestOrder result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns(new HarvestOrder[] { new HarvestOrder() { HarvestOrderID = 1 } });

			module = new HarvestOrderModule(NullLogger.Instance, database);
			result = module.GetHarvestOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.HarvestOrderID);
		}

		[TestMethod]
		public void ShouldGetHarvestOrders()
		{
			HarvestOrderModule module;
			HarvestOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns(new HarvestOrder[] { new HarvestOrder() { HarvestOrderID = 1 }, new HarvestOrder() { HarvestOrderID = 2 }, new HarvestOrder() { HarvestOrderID = 3 } });

			module = new HarvestOrderModule(NullLogger.Instance, database);
			results = module.GetHarvestOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].HarvestOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetHarvestOrdersForFactory()
		{
			HarvestOrderModule module;
			HarvestOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns(new HarvestOrder[] { new HarvestOrder() { HarvestOrderID = 1 }, new HarvestOrder() { HarvestOrderID = 2 }, new HarvestOrder() { HarvestOrderID = 3 } });

			module = new HarvestOrderModule(NullLogger.Instance, database);
			results = module.GetHarvestOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].HarvestOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetWaitingHarvestOrdersForPlanet()
		{
			HarvestOrderModule module;
			HarvestOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns(new HarvestOrder[] { new HarvestOrder() { HarvestOrderID = 1 }, new HarvestOrder() { HarvestOrderID = 2 }, new HarvestOrder() { HarvestOrderID = 3 } });

			module = new HarvestOrderModule(NullLogger.Instance, database);
			results = module.GetWaitingHarvestOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].HarvestOrderID);
			}
		}

		[TestMethod]
		public void ShouldNotGetHarvestOrderAndLogError()
		{
			HarvestOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new HarvestOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetHarvestOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetHarvestOrdersAndLogError()
		{
			HarvestOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new HarvestOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetHarvestOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetHarvestOrdersForFactoryAndLogError()
		{
			HarvestOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new HarvestOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetHarvestOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWaitingHarvestOrdersForPlanetAndLogError()
		{
			HarvestOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<HarvestOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new HarvestOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWaitingHarvestOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreateHarvestOrder()
		{
			HarvestOrderModule module;
			HarvestOrder result;
			IDatabase database;
			int inserted = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => inserted++);

			module = new HarvestOrderModule(NullLogger.Instance, database);

			result = module.CreateHarvestOrder(1,2);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, inserted);
		}

		[TestMethod]
		public void ShouldNotCreateHarvestOrderAndLogError()
		{
			HarvestOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => { throw new Exception(); });

			module = new HarvestOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateHarvestOrder(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}



	}
}
