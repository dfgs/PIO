using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedTakerModule :MockedFunctionalModule,  ITakerModule
	{

		public event TaskCreatedHandler TaskCreated;

		public MockedTakerModule(bool ThrowException) : base( ThrowException)
		{
		}


		public Task BeginTake(int WorkerID,  ResourceTypeIDs ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now,ResourceTypeID=ResourceTypeID };
			TaskCreated?.Invoke(this, new Task[] { task });
			return task;
		}

		

		public void EndTake(int WorkerID,  ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		
	}
}
