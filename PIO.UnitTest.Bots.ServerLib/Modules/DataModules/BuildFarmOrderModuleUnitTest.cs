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
	public class BuildFarmBuildFarmOrderModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetBuildFarmOrder()
		{
			BuildFarmOrderModule module;
			BuildFarmOrder result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns(new BuildFarmOrder[] { new BuildFarmOrder() { BuildFarmOrderID = 1 } });

			module = new BuildFarmOrderModule(NullLogger.Instance, database);
			result = module.GetBuildFarmOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildFarmOrderID);
		}

		[TestMethod]
		public void ShouldGetAllBuildFarmOrders()
		{
			BuildFarmOrderModule module;
			BuildFarmOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns(new BuildFarmOrder[] { new BuildFarmOrder() { BuildFarmOrderID = 1 }, new BuildFarmOrder() { BuildFarmOrderID = 2 }, new BuildFarmOrder() { BuildFarmOrderID = 3 } });

			module = new BuildFarmOrderModule(NullLogger.Instance, database);
			results = module.GetBuildFarmOrders();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildFarmOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetBuildFarmOrdersAtPosition()
		{
			BuildFarmOrderModule module;
			BuildFarmOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns(new BuildFarmOrder[] { new BuildFarmOrder() { BuildFarmOrderID = 1 }, new BuildFarmOrder() { BuildFarmOrderID = 2 }, new BuildFarmOrder() { BuildFarmOrderID = 3 } });

			module = new BuildFarmOrderModule(NullLogger.Instance, database);
			results = module.GetBuildFarmOrders(1,2,2);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildFarmOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetWaitingBuildFarmOrdersForPlanet()
		{
			BuildFarmOrderModule module;
			BuildFarmOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns(new BuildFarmOrder[] { new BuildFarmOrder() { BuildFarmOrderID = 1 }, new BuildFarmOrder() { BuildFarmOrderID = 2 }, new BuildFarmOrder() { BuildFarmOrderID = 3 } });

			module = new BuildFarmOrderModule(NullLogger.Instance, database);
			results = module.GetWaitingBuildFarmOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildFarmOrderID);
			}
		}

		[TestMethod]
		public void ShouldNotGetBuildFarmOrderAndLogError()
		{
			BuildFarmOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFarmOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildFarmOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetAllBuildFarmOrdersAndLogError()
		{
			BuildFarmOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFarmOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildFarmOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetBuildFarmOrdersAtPosAndLogError()
		{
			BuildFarmOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFarmOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildFarmOrders(1,2,2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWaitingBuildFarmOrdersForPlanetAndLogError()
		{
			BuildFarmOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFarmOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFarmOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWaitingBuildFarmOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreateBuildFarmOrder()
		{
			BuildFarmOrderModule module;
			BuildFarmOrder result;
			IDatabase database;
			int inserted = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => inserted++);

			module = new BuildFarmOrderModule(NullLogger.Instance, database);

			result = module.CreateBuildFarmOrder(1,Models.FarmTypeIDs.Forest, 2,2);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, inserted);
		}

		[TestMethod]
		public void ShouldNotCreateBuildFarmOrderAndLogError()
		{
			BuildFarmOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => { throw new Exception(); });

			module = new BuildFarmOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateBuildFarmOrder(1, Models.FarmTypeIDs.Forest, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}



	}
}
