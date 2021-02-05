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
			BuildFactoryOrderModule module;
			BuildFactoryOrder result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns(new BuildFactoryOrder[] { new BuildFactoryOrder() { BuildFactoryOrderID = 1 } });

			module = new BuildFactoryOrderModule(NullLogger.Instance, database);
			result = module.GetBuildFactoryOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildFactoryOrderID);
		}

		[TestMethod]
		public void ShouldGetAllBuildFactoryOrders()
		{
			BuildFactoryOrderModule module;
			BuildFactoryOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns(new BuildFactoryOrder[] { new BuildFactoryOrder() { BuildFactoryOrderID = 1 }, new BuildFactoryOrder() { BuildFactoryOrderID = 2 }, new BuildFactoryOrder() { BuildFactoryOrderID = 3 } });

			module = new BuildFactoryOrderModule(NullLogger.Instance, database);
			results = module.GetBuildFactoryOrders();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildFactoryOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetBuildFactoryOrdersAtPosition()
		{
			BuildFactoryOrderModule module;
			BuildFactoryOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns(new BuildFactoryOrder[] { new BuildFactoryOrder() { BuildFactoryOrderID = 1 }, new BuildFactoryOrder() { BuildFactoryOrderID = 2 }, new BuildFactoryOrder() { BuildFactoryOrderID = 3 } });

			module = new BuildFactoryOrderModule(NullLogger.Instance, database);
			results = module.GetBuildFactoryOrders(1,2,2);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildFactoryOrderID);
			}
		}
		[TestMethod]
		public void ShouldGetWaitingBuildFactoryOrdersForPlanet()
		{
			BuildFactoryOrderModule module;
			BuildFactoryOrder[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns(new BuildFactoryOrder[] { new BuildFactoryOrder() { BuildFactoryOrderID = 1 }, new BuildFactoryOrder() { BuildFactoryOrderID = 2 }, new BuildFactoryOrder() { BuildFactoryOrderID = 3 } });

			module = new BuildFactoryOrderModule(NullLogger.Instance, database);
			results = module.GetWaitingBuildFactoryOrders(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t + 1, results[t].BuildFactoryOrderID);
			}
		}

		[TestMethod]
		public void ShouldNotGetBuildFactoryOrderAndLogError()
		{
			BuildFactoryOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFactoryOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildFactoryOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetAllBuildFactoryOrdersAndLogError()
		{
			BuildFactoryOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFactoryOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildFactoryOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetBuildFactoryOrdersAtPosAndLogError()
		{
			BuildFactoryOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFactoryOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildFactoryOrders(1,2,2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWaitingBuildFactoryOrdersForPlanetAndLogError()
		{
			BuildFactoryOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<BuildFactoryOrder>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BuildFactoryOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWaitingBuildFactoryOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreateBuildFactoryOrder()
		{
			BuildFactoryOrderModule module;
			BuildFactoryOrder result;
			IDatabase database;
			int inserted = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => inserted++);

			module = new BuildFactoryOrderModule(NullLogger.Instance, database);

			result = module.CreateBuildFactoryOrder(1,Models.FactoryTypeIDs.Sawmill, 2,2);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, inserted);
		}

		[TestMethod]
		public void ShouldNotCreateBuildFactoryOrderAndLogError()
		{
			BuildFactoryOrderModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => { throw new Exception(); });

			module = new BuildFactoryOrderModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateBuildFactoryOrder(1, Models.FactoryTypeIDs.Sawmill, 2, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}



	}
}
