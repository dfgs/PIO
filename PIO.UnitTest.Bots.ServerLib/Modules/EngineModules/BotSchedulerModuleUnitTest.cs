using System;
using System.Threading;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PIO.Bots.ServerLib.Modules;
using PIO.ClientLib.PIOServiceReference;
using PIO.Bots.Models.Modules;
using NSubstitute;
using PIO.Models;
using PIO.Bots.Models;
using PIO.ModulesLib.Exceptions;

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class BotSchedulerModuleUnitTest
	{
		[TestMethod]
		public void ShouldCreateBot()
		{
			Bot result;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			botModule = Substitute.For<IBotModule>();
			botModule.CreateBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 } );
			orderManagerModule = Substitute.For<IOrderManagerModule>();
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			
			scheduler = new BotSchedulerModule(NullLogger.Instance, client, botModule, orderManagerModule, 1);
			result = scheduler.CreateBot(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, scheduler.Count);
		}
		[TestMethod]
		public void ShouldNotCreateBotIfAlreadyExistsAndLogErrors()
		{
			MemoryLogger logger;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			botModule = Substitute.For<IBotModule>();
			botModule.CreateBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 });
			botModule.GetBotForWorker(Arg.Any<int>()).Returns((x)=> new Bot() { BotID = 1, WorkerID = 1 });
			orderManagerModule = Substitute.For<IOrderManagerModule>();
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();

			logger = new MemoryLogger();
			scheduler = new BotSchedulerModule(logger, client, botModule, orderManagerModule, 1);
			Assert.ThrowsException<PIOInvalidOperationException>(() => scheduler.CreateBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == scheduler.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotCreateBotAndLogErrors()
		{
			MemoryLogger logger;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			botModule = Substitute.For<IBotModule>();
			botModule.CreateBot(Arg.Any<int>()).Returns((x) => throw new Exception());
			orderManagerModule = Substitute.For<IOrderManagerModule>();
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();

			logger = new MemoryLogger();
			scheduler = new BotSchedulerModule(logger, client, botModule, orderManagerModule, 1);
			Assert.ThrowsException<PIOInvalidOperationException>(() => scheduler.CreateBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == scheduler.ModuleName)));
		}

		[TestMethod]
		public void ShouldDeleteBot()
		{
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;
			int counter = 0;

			botModule = Substitute.For<IBotModule>();
			botModule.GetBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 });
			botModule.When(x=>x.DeleteBot(Arg.Any<int>())).Do(x=>counter++);
			
			orderManagerModule = Substitute.For<IOrderManagerModule>();
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();

			scheduler = new BotSchedulerModule(NullLogger.Instance, client, botModule, orderManagerModule, 1);
			scheduler.DeleteBot(1);
			Assert.AreEqual(1, counter);
		}

		[TestMethod]
		public void ShouldNotDeleteBotWhenSubModuleFailsAndLogErrors()
		{
			MemoryLogger logger;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			botModule = Substitute.For<IBotModule>();
			botModule.GetBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 });
			botModule.When(x => x.DeleteBot(Arg.Any<int>())).Do(x => throw new Exception());

			orderManagerModule = Substitute.For<IOrderManagerModule>();
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();

			logger = new MemoryLogger();
			scheduler = new BotSchedulerModule(logger, client, botModule, orderManagerModule, 1);
			Assert.ThrowsException<PIOInternalErrorException>(() => scheduler.DeleteBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == scheduler.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotDeleteBotWhenDoesntExistAndLogErrors()
		{
			MemoryLogger logger;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			botModule = Substitute.For<IBotModule>();
			botModule.GetBot(Arg.Any<int>()).Returns((x) => null);
			botModule.When(x => x.DeleteBot(Arg.Any<int>())).Do(x=> { });

			orderManagerModule = Substitute.For<IOrderManagerModule>();
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();

			logger = new MemoryLogger();
			scheduler = new BotSchedulerModule(logger, client, botModule, orderManagerModule, 1);
			Assert.ThrowsException<PIONotFoundException>(() => scheduler.DeleteBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == scheduler.ModuleName)));
		}


		[TestMethod]
		public void ShouldCreateTaskIfWorkerIsIdle()
		{
			int taskCreated = 0;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			botModule = Substitute.For<IBotModule>();
			botModule.GetBots().Returns((x) => new Bot[] { new Bot() { BotID = 1, WorkerID = 1 } });
			botModule.GetBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 });
			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task()).AndDoes((x)=>taskCreated++);
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns((x)=>null);
			client.GetAllWorkers().Returns(new Worker[] { new Worker() { WorkerID=1 } });
			scheduler = new BotSchedulerModule(NullLogger.Instance,client, botModule,orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.IsTrue(taskCreated > 0);
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfWorkerIsNotIdle()
		{
			int taskCreated = 0;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			botModule = Substitute.For<IBotModule>();
			botModule.GetBots().Returns((x) => new Bot[] { new Bot() { BotID = 1, WorkerID = 1 } });
			botModule.GetBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 });

			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>(),Arg.Any<int>()).Returns(new Task()).AndDoes((x) => taskCreated++);
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns(new Task());
			client.GetWorkers(Arg.Any<int>()).Returns(new Worker[] { new Worker() { WorkerID = 1 } });

			scheduler = new BotSchedulerModule(NullLogger.Instance, client, botModule, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, taskCreated);
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoCheckIdleState()
		{
			int taskCreated = 0;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;
			MemoryLogger logger;

			logger = new MemoryLogger();
			botModule = Substitute.For<IBotModule>();
			botModule.GetBots().Returns((x) => new Bot[] { new Bot() { BotID = 1, WorkerID = 1 } });
			botModule.GetBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 });

			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>(),Arg.Any<int>()).Returns(new Task()).AndDoes((x) => taskCreated++);
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns((x) => { throw new Exception(); });
			client.GetAllWorkers().Returns(new Worker[] { new Worker() { WorkerID = 1 } });

			scheduler = new BotSchedulerModule(logger, client, botModule, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, taskCreated);
			Assert.IsTrue( logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.ComponentName == scheduler.ModuleName)  ).Count()>0);
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoRunTask()
		{
			int taskCreated = 0;
			BotSchedulerModule scheduler;
			IBotModule botModule;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;
			MemoryLogger logger;

			logger = new MemoryLogger();
			botModule = Substitute.For<IBotModule>();
			botModule.GetBots().Returns((x) => new Bot[] { new Bot() { BotID = 1, WorkerID = 1 } });
			botModule.GetBot(Arg.Any<int>()).Returns((x) => new Bot() { BotID = 1, WorkerID = 1 });

			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>(),Arg.Any<int>()).Returns((x) => { throw new Exception(); });
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns((x) => null);
			client.GetAllWorkers().Returns(new Worker[] { new Worker() { WorkerID = 1 } });

			scheduler = new BotSchedulerModule(logger, client, botModule, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, taskCreated);
			Assert.IsTrue(logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.ComponentName == scheduler.ModuleName)).Count() > 0);
		}



	}
}
