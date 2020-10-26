using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedCarrierModule :MockedFunctionalModule,  ICarrierModule
	{

		public event TaskCreatedHandler TaskCreated;

		public MockedCarrierModule(bool ThrowException) : base( ThrowException)
		{
		}


		public Task BeginCarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now,FactoryID=TargetFactoryID,ResourceTypeID=ResourceTypeID };
			TaskCreated?.Invoke(this, task);
			return task;
		}

		

		public void EndCarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		
	}
}
