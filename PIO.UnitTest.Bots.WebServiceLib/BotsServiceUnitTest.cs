using System;
using System.Linq;
using System.ServiceModel;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIO.Bots.Models;
using PIO.Bots.WebServiceLib;
using PIO.Bots.Models.Modules;
using NSubstitute;
using PIO.ModulesLib.Exceptions;
using PIO.Models;

namespace PIO.UnitTest.Bots.WebServiceLib
{
	[TestClass]
	public class BotsServiceUnitTest
	{

		#region data
		[TestMethod]
		public void ShouldGetBot()
		{
			BotsService service;
			Bot result;
			IBotModule subModule;
			
			subModule = Substitute.For<IBotModule>();
			subModule.GetBot(Arg.Any<int>()).Returns(new Bot() { BotID = 1 });

			service = new BotsService(NullLogger.Instance,  subModule, null, null, null, null,  null, null);
			result = service.GetBot(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BotID);
		}
		[TestMethod]
		public void ShouldNotGetBotAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBotModule subModule;

			subModule = Substitute.For<IBotModule>();
			subModule.GetBot(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, subModule, null, null, null, null, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldGetBotForWorker()
		{
			BotsService service;
			Bot result;
			IBotModule subModule;

			subModule = Substitute.For<IBotModule>();
			subModule.GetBotForWorker(Arg.Any<int>()).Returns(new Bot() { BotID = 1 });

			service = new BotsService(NullLogger.Instance, subModule, null, null, null, null,null, null);
			result = service.GetBotForWorker(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BotID);
		}
		[TestMethod]
		public void ShouldNotGetBotForWorkerAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBotModule subModule;

			subModule = Substitute.For<IBotModule>();
			subModule.GetBotForWorker(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, subModule, null, null, null, null, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetBotForWorker(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldGetBots()
		{
			BotsService service;
			Bot[] result;
			IBotModule subModule;

			subModule = Substitute.For<IBotModule>();
			subModule.GetBots().Returns(new Bot[] { new Bot() { BotID = 1 }, new Bot() { BotID = 2 }, new Bot() { BotID = 3 } });

			service = new BotsService(NullLogger.Instance, subModule, null, null, null, null, null, null);
			result = service.GetBots();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldNotGetBotsAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBotModule subModule;

			subModule = Substitute.For<IBotModule>();
			subModule.GetBots().Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger,  subModule, null, null, null, null, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetBots());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}





		/*[TestMethod]
		public void ShouldGetOrder()
		{
			BotsService service;
			Order result;
			IOrderModule subModule;

			subModule = Substitute.For<IOrderModule>();
			subModule.GetOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });

			service = new BotsService(NullLogger.Instance,null, subModule, null, null, null, null, null);
			result = service.GetOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.OrderID);
		}
		[TestMethod]
		public void ShouldNotGetOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IOrderModule subModule;

			subModule = Substitute.For<IOrderModule>();
			subModule.GetOrder(Arg.Any<int>()).Returns((id)=> { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, subModule, null, null, null, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}*/

		/*[TestMethod]
		public void ShouldGetOrders()
		{
			BotsService service;
			Order[] result;
			IOrderModule subModule;

			subModule = Substitute.For<IOrderModule>();
			subModule.GetOrders().Returns(new Order[] { new Order() { OrderID = 1 }, new Order() { OrderID = 2 }, new Order() { OrderID = 3 } });

			service = new BotsService(NullLogger.Instance, null,  subModule, null, null, null, null, null);
			result = service.GetOrders();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item!=null));
		}
		[TestMethod]
		public void ShouldNotGetOrdersAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IOrderModule subModule;

			subModule = Substitute.For<IOrderModule>();
			subModule.GetOrders().Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, subModule, null, null, null, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}*/


		[TestMethod]
		public void ShouldGetProduceOrder()
		{
			BotsService service;
			ProduceOrder result;
			IProduceOrderModule subModule;

			subModule = Substitute.For<IProduceOrderModule>();
			subModule.GetProduceOrder(Arg.Any<int>()).Returns(new ProduceOrder() { ProduceOrderID = 1 });

			service = new BotsService(NullLogger.Instance, null, null, subModule, null, null,  null, null);
			result = service.GetProduceOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ProduceOrderID);
		}
		[TestMethod]
		public void ShouldNotGetProduceOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IProduceOrderModule subModule;

			subModule = Substitute.For<IProduceOrderModule>();
			subModule.GetProduceOrder(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, subModule, null, null, null,  null);
			Assert.ThrowsException<FaultException>(() => service.GetProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		[TestMethod]
		public void ShouldGetProduceOrders()
		{
			BotsService service;
			ProduceOrder[] result;
			IProduceOrderModule subModule;

			subModule = Substitute.For<IProduceOrderModule>();
			subModule.GetProduceOrders(Arg.Any<int>()).Returns(new ProduceOrder[] { new ProduceOrder() { ProduceOrderID = 1 }, new ProduceOrder() { ProduceOrderID = 2 }, new ProduceOrder() { ProduceOrderID = 3 } });

			service = new BotsService(NullLogger.Instance, null, null, subModule, null, null,  null, null);
			result = service.GetProduceOrders(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldNotGetProduceOrdersAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IProduceOrderModule subModule;

			subModule = Substitute.For<IProduceOrderModule>();
			subModule.GetProduceOrders(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, subModule, null, null, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetProduceOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

	



		[TestMethod]
		public void ShouldGetHarvestOrder()
		{
			BotsService service;
			HarvestOrder result;
			IHarvestOrderModule subModule;

			subModule = Substitute.For<IHarvestOrderModule>();
			subModule.GetHarvestOrder(Arg.Any<int>()).Returns(new HarvestOrder() { HarvestOrderID = 1 });

			service = new BotsService(NullLogger.Instance, null, null, null, subModule, null, null, null);
			result = service.GetHarvestOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.HarvestOrderID);
		}
		[TestMethod]
		public void ShouldNotGetHarvestOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IHarvestOrderModule subModule;

			subModule = Substitute.For<IHarvestOrderModule>();
			subModule.GetHarvestOrder(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, subModule,  null, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetHarvestOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		[TestMethod]
		public void ShouldGetHarvestOrders()
		{
			BotsService service;
			HarvestOrder[] result;
			IHarvestOrderModule subModule;

			subModule = Substitute.For<IHarvestOrderModule>();
			subModule.GetHarvestOrders(Arg.Any<int>()).Returns(new HarvestOrder[] { new HarvestOrder() { HarvestOrderID = 1 }, new HarvestOrder() { HarvestOrderID = 2 }, new HarvestOrder() { HarvestOrderID = 3 } });

			service = new BotsService(NullLogger.Instance, null, null, null, subModule,  null, null, null);
			result = service.GetHarvestOrders(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldNotGetHarvestOrdersAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IHarvestOrderModule subModule;

			subModule = Substitute.For<IHarvestOrderModule>();
			subModule.GetHarvestOrders(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, subModule, null,  null, null);
			Assert.ThrowsException<FaultException>(() => service.GetHarvestOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		



		[TestMethod]
		public void ShouldGetBuildOrder()
		{
			BotsService service;
			BuildOrder result;
			IBuildOrderModule subModule;

			subModule = Substitute.For<IBuildOrderModule>();
			subModule.GetBuildOrder(Arg.Any<int>()).Returns(new BuildOrder() { BuildOrderID = 1 });

			service = new BotsService(NullLogger.Instance, null, null, null, null, subModule,  null,  null);
			result = service.GetBuildOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildOrderID);
		}
		[TestMethod]
		public void ShouldNotGetBuildOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBuildOrderModule subModule;

			subModule = Substitute.For<IBuildOrderModule>();
			subModule.GetBuildOrder(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, subModule, null,  null);
			Assert.ThrowsException<FaultException>(() => service.GetBuildOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetBuildOrders()
		{
			BotsService service;
			BuildOrder[] result;
			IBuildOrderModule subModule;

			subModule = Substitute.For<IBuildOrderModule>();
			subModule.GetBuildOrders(Arg.Any<int>()).Returns(new BuildOrder[] { new BuildOrder() { BuildOrderID = 1 }, new BuildOrder() { BuildOrderID = 2 }, new BuildOrder() { BuildOrderID = 3 } });

			service = new BotsService(NullLogger.Instance, null, null, null, null, subModule,  null, null);
			result = service.GetBuildOrders(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldNotGetBuildOrdersAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBuildOrderModule subModule;

			subModule = Substitute.For<IBuildOrderModule>();
			subModule.GetBuildOrders(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, subModule, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetBuildOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetBuildOrdersAtPosition()
		{
			BotsService service;
			BuildOrder[] result;
			IBuildOrderModule subModule;

			subModule = Substitute.For<IBuildOrderModule>();
			subModule.GetBuildOrders(Arg.Any<int>(), Arg.Any<int>(),Arg.Any<int>() ).Returns(new BuildOrder[] { new BuildOrder() { BuildOrderID = 1 }, new BuildOrder() { BuildOrderID = 2 }, new BuildOrder() { BuildOrderID = 3 } });

			service = new BotsService(NullLogger.Instance, null, null, null, null, subModule, null, null);
			result = service.GetBuildOrdersAtPosition(1,2,3) ;
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldNotGetBuildOrdersAtPositionAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBuildOrderModule subModule;

			subModule = Substitute.For<IBuildOrderModule>();
			subModule.GetBuildOrders(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, subModule, null,  null);
			Assert.ThrowsException<FaultException>(() => service.GetBuildOrdersAtPosition(1,2,3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		#endregion

		#region functional

		[TestMethod]
		public void ShouldCreateBot()
		{
			BotsService service;
			Bot result;
			IBotSchedulerModule subModule;

			subModule = Substitute.For<IBotSchedulerModule>();
			subModule.CreateBot(Arg.Any<int>()).Returns(new Bot() { BotID = 1 });

			service = new BotsService(NullLogger.Instance, null, null, null, null, null, subModule, null);
			result = service.CreateBot(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BotID);
		}
		
		[TestMethod]
		public void ShouldNotCreateBotAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBotSchedulerModule subModule;

			subModule = Substitute.For<IBotSchedulerModule>();
			subModule.CreateBot(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, null,  subModule, null);
			Assert.ThrowsException<FaultException>(() => service.CreateBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldDeleteBot()
		{
			BotsService service;
			IBotSchedulerModule subModule;
			int counter = 0;

			subModule = Substitute.For<IBotSchedulerModule>();
			subModule.When((del) => del.DeleteBot(Arg.Any<int>())).Do((del) => { counter++; }) ;

			service = new BotsService(NullLogger.Instance, null, null, null, null, null, subModule, null);
			service.DeleteBot(1);
			Assert.AreEqual(1, counter);
		}

		[TestMethod]
		public void ShouldNotDeleteBotAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IBotSchedulerModule subModule;

			subModule = Substitute.For<IBotSchedulerModule>();
			subModule.When((del) => del.DeleteBot(Arg.Any<int>())).Do((del) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, null, subModule, null);
			Assert.ThrowsException<FaultException>(() => service.DeleteBot(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}



		[TestMethod]
		public void ShouldCreateBuildOrder()
		{
			BotsService service;
			BuildOrder result;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateBuildOrder(Arg.Any<int>(),Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildOrder() { BuildOrderID = 1 });

			service = new BotsService(NullLogger.Instance, null, null, null, null, null, null, subModule);
			result = service.CreateBuildOrder(1,BuildingTypeIDs.Forest,2,3);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildOrderID);
		}

		[TestMethod]
		public void ShouldNotCreateBuildOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, null, null, subModule);
			Assert.ThrowsException<FaultException>(() => service.CreateBuildOrder(1, BuildingTypeIDs.Forest, 2, 3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldCreateProduceOrder()
		{
			BotsService service;
			ProduceOrder result;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateProduceOrder(Arg.Any<int>(),  Arg.Any<int>()).Returns(new ProduceOrder() { ProduceOrderID = 1 });

			service = new BotsService(NullLogger.Instance, null, null, null, null, null, null, subModule);
			result = service.CreateProduceOrder(1,  2);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ProduceOrderID);
		}

		[TestMethod]
		public void ShouldNotCreateProduceOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, null, null, subModule);
			Assert.ThrowsException<FaultException>(() => service.CreateProduceOrder(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldCreateHarvestOrder()
		{
			BotsService service;
			HarvestOrder result;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateHarvestOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new HarvestOrder() { HarvestOrderID = 1 });

			service = new BotsService(NullLogger.Instance, null, null, null, null, null, null, subModule);
			result = service.CreateHarvestOrder(1, 2);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.HarvestOrderID);
		}

		[TestMethod]
		public void ShouldNotCreateHarvestOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateHarvestOrder(Arg.Any<int>(), Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, null, null, null, null, subModule);
			Assert.ThrowsException<FaultException>(() => service.CreateHarvestOrder(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		#endregion



	}
}
