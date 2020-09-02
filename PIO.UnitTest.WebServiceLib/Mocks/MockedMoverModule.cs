using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedMoverModule :MockedFunctionalModule,  IMoverModule
	{


		public MockedMoverModule(bool ThrowException) : base( ThrowException)
		{
		}

		public event TaskCreatedHandler TaskCreated;

		public Task BeginMoveTo(int WorkerID,int TargetFactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task= new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, task);
			return task;
		}

		public void EndMoveTo(int WorkerID,int TargetFactoryID)
		{
			throw new NotImplementedException();
		}
	}
}
