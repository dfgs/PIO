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

			service = new BotsService(NullLogger.Instance, new MockedOrderModule(3, false));
			result = service.GetOrder(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.OrderID);
		}
		[TestMethod]
		public void ShouldGetOrders()
		{
			BotsService service;
			Order[] result;

			service = new BotsService(NullLogger.Instance, new MockedOrderModule(3, false));
			result = service.GetOrders();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item!=null));
		}

		


	}
}
