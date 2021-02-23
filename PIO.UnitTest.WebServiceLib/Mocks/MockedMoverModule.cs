using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
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

		public Task[] BeginMoveTo(int WorkerID,int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task= new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, new Task[] { task });
			return new Task[] { task };
		}
		public Task[] BeginMoveTo(int WorkerID, int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, new Task[] { task });
			return new Task[] { task };
		}
		public void EndMoveTo(int WorkerID,int X,int Y)
		{
			throw new NotImplementedException();
		}
	}
}
