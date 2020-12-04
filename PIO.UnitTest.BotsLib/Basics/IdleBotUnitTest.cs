using System;
using System.Threading;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIO.BotsLib.Basic;
using PIO.UnitTest.BotsLib.Mocks;
using System.Linq;

namespace PIO.UnitTest.BotsLib.Basics
{
	[TestClass]
	public class IdleBotUnitTest
	{
		[TestMethod]
		public void ShouldLogErrorsWhenFailsToFindIfWorkerIsIdle()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			IdleBot bot;

			client = new MockedPIOClient(true,false,false);
			logger = new MemoryLogger();
			bot = new IdleBot(logger, client, 1, 1);

			Assert.IsTrue(bot.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(bot.Stop());

			Assert.IsTrue(logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("GETLASTTASK ERROR"))).Count()>0  );
			Assert.AreEqual(0, logger.Logs.Where(item => (item.Level == LogLevels.Warning) && (item.Message.Contains("Worker is not idle"))).Count());
			Assert.AreEqual(0,logger.Logs.Where(item => (item.Level == LogLevels.Information) && (item.Message.Contains("Running new task"))).Count());


		}

		[TestMethod]
		public void ShouldWaitExistingTaskToFinish()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			IdleBot bot;

			client = new MockedPIOClient(false,true, false);
			logger = new MemoryLogger();
			bot = new IdleBot(logger, client, 1, 1);

			Assert.IsTrue(bot.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(bot.Stop());

			Assert.AreEqual(0,logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("GETLASTTASK ERROR"))).Count());
			Assert.AreEqual(1,logger.Logs.Where(item => (item.Level == LogLevels.Warning) && (item.Message.Contains("Worker is not idle"))).Count() );


		}

		[TestMethod]
		public void ShouldNotWaitExistingTaskAndGenerateTasks()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			IdleBot bot;

			client = new MockedPIOClient(false,false, false);
			logger = new MemoryLogger();
			bot = new IdleBot(logger, client, 1, 1);

			Assert.IsTrue(bot.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(bot.Stop());

			Assert.AreEqual(0, logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("GETLASTTASK ERROR"))).Count());
			Assert.AreEqual(0,logger.Logs.Where(item => (item.Level == LogLevels.Warning) && (item.Message.Contains("Worker is not idle"))).Count() );
			Assert.IsTrue(logger.Logs.Where(item => (item.Level == LogLevels.Information) && (item.Message.Contains("Running new task"))).Count() != 0);
	
		}


		[TestMethod]
		public void ShouldLogErrorWhenFailsToGenerateTasks()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			IdleBot bot;

			client = new MockedPIOClient(false, false,true);
			logger = new MemoryLogger();
			bot = new IdleBot(logger, client, 1, 1);

			Assert.IsTrue(bot.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(bot.Stop());

			Assert.IsTrue(logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("IDLE ERROR"))).Count() > 0);
			Assert.AreEqual(0, logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("GETLASTTASK ERROR"))).Count());
			Assert.AreEqual(0, logger.Logs.Where(item => (item.Level == LogLevels.Warning) && (item.Message.Contains("Worker is not idle"))).Count());

		}


	}
}
