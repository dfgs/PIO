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

namespace PIO.UnitTest.Bots.WebServiceLib
{
	[TestClass]
	public class BotsServiceUnitTest
	{

		[TestMethod]
		public void ShouldGetOrder()
		{
			BotsService service;
			Order result;
			IOrderModule subModule;

			subModule = Substitute.For<IOrderModule>();
			subModule.GetOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });

			service = new BotsService(NullLogger.Instance, subModule,null,null);
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
			service = new BotsService(logger, subModule, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		[TestMethod]
		public void ShouldGetOrders()
		{
			BotsService service;
			Order[] result;
			IOrderModule subModule;

			subModule = Substitute.For<IOrderModule>();
			subModule.GetOrders().Returns(new Order[] { new Order() { OrderID = 1 }, new Order() { OrderID = 2 }, new Order() { OrderID = 3 } });

			service = new BotsService(NullLogger.Instance,subModule, null, null);
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
			service = new BotsService(logger, subModule, null, null);
			Assert.ThrowsException<FaultException>(() => service.GetOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetProduceOrder()
		{
			BotsService service;
			ProduceOrder result;
			IProduceOrderModule subModule;

			subModule = Substitute.For<IProduceOrderModule>();
			subModule.GetProduceOrder(Arg.Any<int>()).Returns(new ProduceOrder() { ProduceOrderID = 1 });

			service = new BotsService(NullLogger.Instance, null, subModule,  null);
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
			service = new BotsService(logger, null, subModule,  null);
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
			subModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { ProduceOrderID = 1 }, new ProduceOrder() { ProduceOrderID = 2 }, new ProduceOrder() { ProduceOrderID = 3 } });

			service = new BotsService(NullLogger.Instance, null, subModule, null);
			result = service.GetProduceOrders();
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
			subModule.GetProduceOrders().Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, subModule,  null);
			Assert.ThrowsException<FaultException>(() => service.GetProduceOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreateProduceOrder()
		{
			BotsService service;
			ProduceOrder result;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateProduceOrder(Arg.Any<int>()).Returns(new ProduceOrder() {ProduceOrderID=1 ,FactoryID=1});

			service = new BotsService(NullLogger.Instance, null, null, subModule);
			result=service.CreateProduceOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);
		}

		[TestMethod]
		public void ShouldNotCreateProduceAndLogError()
		{
			MemoryLogger logger;
			BotsService service;
			IOrderManagerModule subModule;

			subModule = Substitute.For<IOrderManagerModule>();
			subModule.CreateProduceOrder(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, subModule);
			Assert.ThrowsException<FaultException>(() => service.CreateProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}






	}
}
