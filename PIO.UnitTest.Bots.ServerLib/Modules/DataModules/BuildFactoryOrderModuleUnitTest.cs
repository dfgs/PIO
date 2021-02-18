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
	public class BuildFactoryBuildFactoryOrderModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetBuildFactoryOrder()
		{
			BuildOrderModule module;
			BuildOrder result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns(new BuildOrder[] { new BuildOrder() { BuildOrderID = 1 } });

			module = new BuildOrderModule(NullLogger.Instance, database);
			result = module.GetBuildOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildOrderID);
		}

		[TestMethod]
		public void ShouldGetAllBuildFactoryOrders()
		{
			BuildOrderModule module;
			BuildOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns(new BuildOrder[] { new BuildOrder() { BuildOrderID = 1 }, new BuildOrder() { BuildOrderID = 2 }, new BuildOrder() { BuildOrderID = 3 } });

			module = new BuildOrderModule(NullLogger.Instance, database);
			results = module.GetBuildOrders();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetBuildFactoryOrdersAtPosition()
		{
			BuildOrderModule module;
			BuildOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns(new BuildOrder[] { new BuildOrder() { BuildOrderID = 1 }, new BuildOrder() { BuildOrderID = 2 }, new BuildOrder() { BuildOrderID = 3 } });

			module = new BuildOrderModule(NullLogger.Instance, database);
			results = module.GetBuildOrders(1,2,2);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetWaitingBuildFactoryOrdersForPlanet()
		{
			BuildOrderModule module;
			BuildOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns(new BuildOrder[] { new BuildOrder() { BuildOrderID = 1 }, new BuildOrder() { BuildOrderID = 2 }, new BuildOrder() { BuildOrderID = 3 } });

			module = new BuildOrderModule(NullLogger.Instance, database);
			results = module.GetWaitingBuildOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildOrderID);
			}
		}

		[TestMethod]
		public void ShouldNotGetBuildFactoryOrderAndLogError()
		{
			BuildOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetAllBuildFactoryOrdersAndLogError()
		{
			BuildOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetBuildFactoryOrdersAtPosAndLogError()
		{
			BuildOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildOrders(1,2,2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWaitingBuildFactoryOrdersForPlanetAndLogError()
		{
			BuildOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWaitingBuildOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreateBuildFactoryOrder()
		{
			BuildOrderModule module;
			BuildOrder result;
			IDatabase database;
			int inserted = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => inserted++);

			module = new BuildOrderModule(NullLogger.Instance, database);

			result = module.CreateBuildOrder(1,Models.BuildingTypeIDs.Sawmill, 2,2);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, inserted);
		}

		[TestMethod]
		public void ShouldNotCreateBuildFactoryOrderAndLogError()
		{
			BuildOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => { throw new Exception(); });

			module = new BuildOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateBuildOrder(1, Models.BuildingTypeIDs.Sawmill, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}



	}
}
