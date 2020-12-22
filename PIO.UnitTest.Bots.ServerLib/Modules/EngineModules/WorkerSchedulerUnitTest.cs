using System;
using System.Threading;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PIO.Bots.ServerLib.Modules;
using PIO.UnitTest.Bots.ServiceLib.Mocks;
using PIO.ClientLib.PIOServiceReference;

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class WorkerSchedulerUnitTest
	{
		[TestMethod]
		public void ShouldCreateTaskIfWorkerIsIdle()
		{
			IPIOService client;
			MockedOrderManagerModule orderManagerModule;
			WorkerScheduler scheduler;

			client = new MockedPIOService(false,null, false, false);
			orderManagerModule = new MockedOrderManagerModule(false);

			scheduler = new WorkerScheduler(NullLogger.Instance,client,orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.IsTrue(orderManagerModule.TaskCreated > 0);
		}

		[TestMethod]
		public void ShouldNotCreateTaskIfWorkerIsNotIdle()
		{
			IPIOService client;
			MockedOrderManagerModule orderManagerModule;
			WorkerScheduler scheduler;

			client = new MockedPIOService(false,new Models.Task(), false, false);
			orderManagerModule = new MockedOrderManagerModule(false);

			scheduler = new WorkerScheduler(NullLogger.Instance, client, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, orderManagerModule.TaskCreated);
		}
		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoCheckIdleState()
		{
			IPIOService client;
			MockedOrderManagerModule orderManagerModule;
			WorkerScheduler scheduler;
			MemoryLogger logger;

			logger = new MemoryLogger();
			client = new MockedPIOService(true,null, false, false);
			orderManagerModule = new MockedOrderManagerModule(false);

			scheduler = new WorkerScheduler(logger, client, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, orderManagerModule.TaskCreated);
			Assert.IsTrue( logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("GetCurrentTask ERROR"))  ).Count()>0);
		}
		[TestMethod]
		public void ShouldNotCreateTaskIfFailstoRunTask()
		{
			IPIOService client;
			MockedOrderManagerModule orderManagerModule;
			WorkerScheduler scheduler;
			MemoryLogger logger;

			logger = new MemoryLogger();
			client = new MockedPIOService(false,null, false, false);
			orderManagerModule = new MockedOrderManagerModule(true);

			scheduler = new WorkerScheduler(logger, client, orderManagerModule, 1);
			Assert.IsTrue(scheduler.Start());
			scheduler.Add(1);
			Thread.Sleep(2000);
			Assert.IsTrue(scheduler.Stop());
			Assert.AreEqual(0, orderManagerModule.TaskCreated);
			Assert.IsTrue(logger.Logs.Where(item => (item.Level == LogLevels.Error) && (item.Message.Contains("RunTask ERROR"))).Count() > 0);
		}



	}
}
