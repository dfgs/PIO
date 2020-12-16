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

			service = new BotsService(NullLogger.Instance, new MockedOrderModule(3, false),null);
			result = service.GetOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.OrderID);
		}
		[TestMethod]
		public void ShouldGetOrders()
		{
			BotsService service;
			Order[] result;

			service = new BotsService(NullLogger.Instance, new MockedOrderModule(3, false), null);
			result = service.GetOrders();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item!=null));
		}


		[TestMethod]
		public void ShouldGetProduceOrder()
		{
			BotsService service;
			ProduceOrder result;

			service = new BotsService(NullLogger.Instance,null, new MockedProduceOrderModule(3, false));
			result = service.GetProduceOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ProduceOrderID);
		}
		[TestMethod]
		public void ShouldGetProduceOrders()
		{
			BotsService service;
			ProduceOrder[] result;

			service = new BotsService(NullLogger.Instance, null, new MockedProduceOrderModule(3, false));
			result = service.GetProduceOrders();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}




	}
}
