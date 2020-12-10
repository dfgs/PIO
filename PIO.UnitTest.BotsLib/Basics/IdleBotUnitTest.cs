using System;
using System.Threading;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIO.BotsLib.Basic;
using PIO.UnitTest.BotsLib.Mocks;
using System.Linq;
using PIO.Models;
using PIO.BotsLib;

namespace PIO.UnitTest.BotsLib.Basics
{
	[TestClass]
	public class IdleBotUnitTest
	{

		[TestMethod]
		public void ShouldGetCurrentTask()
		{
			MockedPIOClient client;
			IdleBot bot;
			Task task;

			client = new MockedPIOClient(false);
			bot = new IdleBot(NullLogger.Instance, client, 1, 1);

			task=bot.GetCurrentTask();
			Assert.IsNotNull(task);

		}

		[TestMethod]
		public void ShouldNotGetCurrentTask()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			IdleBot bot;


			client = new MockedPIOClient(true);
			logger = new MemoryLogger();
			bot = new IdleBot(logger, client, 1, 1);

			Assert.ThrowsException<BotException>(()=> bot.GetCurrentTask());
		}

		[TestMethod]
		public void ShouldCreateTask()
		{
			MockedPIOClient client;
			IdleBot bot;
			Task task;

			client = new MockedPIOClient(false);
			bot = new IdleBot(NullLogger.Instance, client, 1, 1);

			task = bot.RunTask();
			Assert.IsNotNull(task);

		}

		[TestMethod]
		public void ShouldNotCreateTask()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			IdleBot bot;


			client = new MockedPIOClient(true);
			logger = new MemoryLogger();
			bot = new IdleBot(logger, client, 1, 1);

			Assert.ThrowsException<BotException>(() => bot.RunTask());
		}




	}
}
