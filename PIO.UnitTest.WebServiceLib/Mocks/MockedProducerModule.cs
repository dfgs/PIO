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

		public event TaskCreatedHandler TaskCreated;

		public Task BeginProduce(int WorkerID,int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task= new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, task);
			return task;
		}

		public void EndProduce(int WorkerID, int FactoryID)
		{
			throw new NotImplementedException();
		}
	}
}
