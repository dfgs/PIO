using System;
using System.Linq;
using System.ServiceModel;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIO.Bots.Models;
using PIO.UnitTest.Bots.WebServiceLib.Mocks;
using PIO.Bots.WebServiceLib;

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

			service = new BotsService(NullLogger.Instance, new MockedOrderModule(3, false),null,null);
			result = service.GetOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.OrderID);
		}
		[TestMethod]
		public void ShouldNotGetOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;

			logger = new MemoryLogger();
			service = new BotsService(logger, new MockedOrderModule(3, true), null, null);
			Assert.ThrowsException<FaultException>(() => service.GetOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		[TestMethod]
		public void ShouldGetOrders()
		{
			BotsService service;
			Order[] result;

			service = new BotsService(NullLogger.Instance, new MockedOrderModule(3, false), null, null);
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

			logger = new MemoryLogger();
			service = new BotsService(logger, new MockedOrderModule(3, true), null, null);
			Assert.ThrowsException<FaultException>(() => service.GetOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		[TestMethod]
		public void ShouldGetProduceOrder()
		{
			BotsService service;
			ProduceOrder result;

			service = new BotsService(NullLogger.Instance,null, new MockedProduceOrderModule(3, false), null);
			result = service.GetProduceOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ProduceOrderID);
		}

		[TestMethod]
		public void ShouldNotGetProduceOrderAndLogError()
		{
			MemoryLogger logger;
			BotsService service;

			logger = new MemoryLogger();
			service = new BotsService(logger, null, new MockedProduceOrderModule(3, true), null);
			Assert.ThrowsException<FaultException>(() => service.GetProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}

		[TestMethod]
		public void ShouldGetProduceOrders()
		{
			BotsService service;
			ProduceOrder[] result;

			service = new BotsService(NullLogger.Instance, null, new MockedProduceOrderModule(3, false), null);
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

			logger = new MemoryLogger();
			service = new BotsService(logger, null, new MockedProduceOrderModule(3, true), null);
			Assert.ThrowsException<FaultException>(() => service.GetProduceOrders());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}




		[TestMethod]
		public void ShouldCreateProduceOrder()
		{
			BotsService service;
			ProduceOrder result;

			service = new BotsService(NullLogger.Instance, null, null, new MockedOrderManagerModule(false));
			result=service.CreateProduceOrder(1);
			Assert.IsNull(result);
			Assert.AreEqual(1, result.FactoryID);
		}

		[TestMethod]
		public void ShouldNotCreateProduceAndLogError()
		{
			MemoryLogger logger;
			BotsService service;

			logger = new MemoryLogger();
			service = new BotsService(logger, null, null, new MockedOrderManagerModule(true));
			Assert.ThrowsException<FaultException>(() => service.CreateProduceOrder(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}




	}
}
