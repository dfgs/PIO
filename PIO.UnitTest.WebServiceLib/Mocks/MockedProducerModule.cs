using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedProducerModule :MockedFunctionalModule,  IProducerModule
	{


		public MockedProducerModule(bool ThrowException) : base( ThrowException)
		{
		}

		public Task Produce(int WorkerID,int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Task() { WorkerID=WorkerID,ETA=DateTime.Now };
		}

	}
}
