using System;
using System.Threading;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIO.BotsLib;
using PIO.UnitTest.BotsLib.Mocks;
using System.Linq;

namespace PIO.UnitTest.BotsLib
{
	[TestClass]
	public class BotSchedulerUnitTest
	{
		[TestMethod]
		public void ShouldCreateTaskIfWorkerIsIdle()
		{
			MockedBot bot;
			BotScheduler scheduler;

			bot = new MockedBot(true, false, false);
			scheduler = new BotScheduler(NullLogger.Instance,1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(bot);
			Thread.Sleep(2000);
			Assert.IsTrue(bot.TaskCreated > 0);
			Assert.IsTrue(scheduler.Stop());
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfWorkerIsNotIdle()
		{
			MockedBot bot;
			BotScheduler scheduler;

			bot = new MockedBot(false, false, false);
			scheduler = new BotScheduler(NullLogger.Instance, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(bot);
			Thread.Sleep(2000);
			Assert.AreEqual(0,bot.TaskCreated);
			Assert.IsTrue(scheduler.Stop());
		}
		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoCheckIdleState()
		{
			MockedBot bot;
			BotScheduler scheduler;
			MemoryLogger logger;

			logger = new MemoryLogger();
			bot = new MockedBot(true, true, false);
			scheduler = new BotScheduler(logger, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(bot);
			Thread.Sleep(2000);
			Assert.AreEqual(0, bot.TaskCreated);
			Assert.IsTrue( logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("GetCurrentTask ERROR"))  ).Count()>0);
			Assert.IsTrue(scheduler.Stop());
		}
		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoRunTask()
		{
			MockedBot bot;
			BotScheduler scheduler;
			MemoryLogger logger;

			logger = new MemoryLogger();
			bot = new MockedBot(true, false, true);
			scheduler = new BotScheduler(logger, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(bot);
			Thread.Sleep(2000);
			Assert.AreEqual(0, bot.TaskCreated);
			Assert.IsTrue(logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("RunTask ERROR"))).Count() > 0);
			Assert.IsTrue(scheduler.Stop());
		}



	}
}
