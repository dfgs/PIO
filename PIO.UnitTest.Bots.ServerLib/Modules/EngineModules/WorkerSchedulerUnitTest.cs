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

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class WorkerSchedulerUnitTest
	{
		[TestMethod]
		public void ShouldCreateTaskIfWorkerIsIdle()
		{
			int taskCreated = 0;
			WorkerScheduler scheduler;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>()).Returns(new Task()).AndDoes((x)=>taskCreated++);
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns((x)=>null);

			scheduler = new WorkerScheduler(NullLogger.Instance,client,orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.IsTrue(taskCreated > 0);
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfWorkerIsNotIdle()
		{
			int taskCreated = 0;
			WorkerScheduler scheduler;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;

			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>()).Returns(new Task()).AndDoes((x) => taskCreated++);
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns(new Task());

			scheduler = new WorkerScheduler(NullLogger.Instance, client, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, taskCreated);
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoCheckIdleState()
		{
			int taskCreated = 0;
			WorkerScheduler scheduler;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;
			MemoryLogger logger;

			logger = new MemoryLogger();
			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>()).Returns(new Task()).AndDoes((x) => taskCreated++);
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns((x) => { throw new Exception(); });

			scheduler = new WorkerScheduler(logger, client, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, taskCreated);
			Assert.IsTrue( logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.ComponentName == scheduler.ModuleName)  ).Count()>0);
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoRunTask()
		{
			int taskCreated = 0;
			WorkerScheduler scheduler;
			IOrderManagerModule orderManagerModule;
			PIO.ClientLib.PIOServiceReference.IPIOService client;
			MemoryLogger logger;

			logger = new MemoryLogger();
			orderManagerModule = Substitute.For<IOrderManagerModule>();
			orderManagerModule.CreateTask(Arg.Any<int>()).Returns((x) => { throw new Exception(); });
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetLastTask(Arg.Any<int>()).Returns((x) => null);

			scheduler = new WorkerScheduler(logger, client, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, taskCreated);
			Assert.IsTrue(logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.ComponentName == scheduler.ModuleName)).Count() > 0);
		}



	}
}
