using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.Bots.ServiceLib.Mocks
{
	public class MockedOrderModule : MockedDatabaseModule, IOrderModule
	{
		public MockedOrderModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public Order GetOrder(int OrderID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Order() { OrderID = OrderID };
		}

	
		public Order[] GetOrders()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Order() { OrderID = t });
		}
		
		public Order CreateOrder()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Order() { OrderID = Count };
		}




	}
}
