using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedIdlerModule :MockedFunctionalModule,  IIdlerModule
	{


		public MockedIdlerModule(bool ThrowException) : base( ThrowException)
		{
		}

		public event TaskCreatedHandler TaskCreated;

		public Task BeginIdle(int WorkerID, int Duration)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task= new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, new Task[] { task });
			return task;
		}

		public void EndIdle(int WorkerID)
		{
			throw new NotImplementedException();
		}
	}
}
