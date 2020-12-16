using PIO.Bots.Models;

using PIO.Bots.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.Bots.WebServiceLib.Mocks
{
	public class MockedProduceOrderModule : MockedDatabaseModule, IProduceOrderModule
	{
		public MockedProduceOrderModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public ProduceOrder GetProduceOrder(int ProduceOrderID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new ProduceOrder() { ProduceOrderID = ProduceOrderID };
		}

		
		public ProduceOrder[] GetProduceOrders()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new ProduceOrder() { ProduceOrderID = t });
		}
		

	}
}
