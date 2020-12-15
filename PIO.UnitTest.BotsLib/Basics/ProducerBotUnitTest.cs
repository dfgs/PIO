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
	public class ProducerBotUnitTest
	{

		[TestMethod]
		public void ShouldGetCurrentTask()
		{
			MockedPIOClient client;
			ProducerBot bot;
			Task task;

			client = new MockedPIOClient(false);
			bot = new ProducerBot(NullLogger.Instance, client, 1, 1);

			task=bot.GetCurrentTask();
			Assert.IsNotNull(task);

		}

		[TestMethod]
		public void ShouldNotGetCurrentTask()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			ProducerBot bot;


			client = new MockedPIOClient(true);
			logger = new MemoryLogger();
			bot = new ProducerBot(logger, client, 1, 1);

			Assert.ThrowsException<BotException>(()=> bot.GetCurrentTask());
		}

		[TestMethod]
		public void ShouldCreateTask()
		{
			MockedPIOClient client;
			ProducerBot bot;
			Task task;

			client = new MockedPIOClient(false);
			bot = new ProducerBot(NullLogger.Instance, client, 1, 1);

			task = bot.RunTask();
			Assert.IsNotNull(task);
			Assert.IsTrue(client.ProduceCount > 0);

		}

		[TestMethod]
		public void ShouldNotCreateTask()
		{
			MemoryLogger logger;
			MockedPIOClient client;
			ProducerBot bot;


			client = new MockedPIOClient(true);
			logger = new MemoryLogger();
			bot = new ProducerBot(logger, client, 1, 1);

			Assert.ThrowsException<BotException>(() => bot.RunTask());
			Assert.AreEqual(0, client.ProduceCount);
		}




	}
}
