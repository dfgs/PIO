using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.Bots.WebServiceLib.Mocks
{
	public class MockedOrderManagerModule : MockedFunctionalModule,IOrderManagerModule
	{
		int produceOrderCount;
		public int ProduceOrderCount => produceOrderCount;

		public MockedOrderManagerModule(bool ThrowException) : base(ThrowException)
		{
		}


		public ProduceOrder CreateProduceOrder(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			produceOrderCount++;
			return new ProduceOrder() { FactoryID = FactoryID };
		}

		public Models.Task CreateTask(int WorkerID)
		{
			throw new NotImplementedException();
		}
	}
}
