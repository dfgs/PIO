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
	public class BotModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetBot()
		{
			BotModule module;
			Bot result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<Bot>(Arg.Any<ISelect>()).Returns(new Bot[] { new Bot() { BotID = 1 } });

			module = new BotModule(NullLogger.Instance, database);
			result = module.GetBot(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BotID);
		}
		[TestMethod]
		public void ShouldGetBotForWorker()
		{
			BotModule module;
			Bot result;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<Bot>(Arg.Any<ISelect>()).Returns(new Bot[] { new Bot() { BotID = 1 } });

			module = new BotModule(NullLogger.Instance, database);
			result = module.GetBotForWorker(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BotID);
		}
		[TestMethod]
		public void ShouldGetAllBots()
		{
			BotModule module;
			Bot[] results;
			IDatabase database;

			database = Substitute.For<IDatabase>();
			database.Execute<Bot>(Arg.Any<ISelect>()).Returns(new Bot[] { new Bot() { BotID = 1 }, new Bot() { BotID = 2 }, new Bot() { BotID = 3 } });

			module = new BotModule(NullLogger.Instance, database);
			results = module.GetBots();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t+1, results[t].BotID);
			}
		}

		[TestMethod]
		public void ShouldNotGetBotAndLogError()
		{
			BotModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<Bot>(Arg.Any<ISelect>()).Returns((x)=> { throw new Exception(); });

			module = new BotModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBotForWorkerAndLogError()
		{
			BotModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<Bot>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BotModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBotForWorker(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetAllBotsAndLogError()
		{
			BotModule module;
			MemoryLogger logger;
			IDatabase database;

			logger = new MemoryLogger();

			database = Substitute.For<IDatabase>();
			database.Execute<Bot>(Arg.Any<ISelect>()).Returns((x) => { throw new Exception(); });

			module = new BotModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBots());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}



		[TestMethod]
		public void ShouldCreateBot()
		{
			BotModule module;
			Bot result;
			IDatabase database;
			int counter = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => counter++);

			module = new BotModule(NullLogger.Instance, database);
			result = module.CreateBot(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			
			Assert.AreEqual(1, counter);
		}

		[TestMethod]
		public void ShouldNotCreateBotAndLogError()
		{
			BotModule module;
			MemoryLogger logger;
			IDatabase database;


			logger = new MemoryLogger();
			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IQuery[]>())).Do(x => throw new Exception());

			module = new BotModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}



		[TestMethod]
		public void ShouldDeleteBot()
		{
			BotModule module;
			IDatabase database;
			int counter = 0;

			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IDelete>())).Do(x => counter++);
			module = new BotModule(NullLogger.Instance, database);
			module.DeleteBot(1);
			Assert.AreEqual(1, counter);
		}

		[TestMethod]
		public void ShouldNotDeleteBotAndLogError()
		{
			BotModule module;
			MemoryLogger logger;
			IDatabase database;


			logger = new MemoryLogger();
			database = Substitute.For<IDatabase>();
			database.When(x => x.Execute(Arg.Any<IDelete>())).Do(x => throw new Exception());
			module = new BotModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.DeleteBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}





	}
}
