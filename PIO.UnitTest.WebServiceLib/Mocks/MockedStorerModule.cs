using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedStorerModule :MockedFunctionalModule,  IStorerModule
	{

		public event TaskCreatedHandler TaskCreated;

		public MockedStorerModule(bool ThrowException) : base( ThrowException)
		{
		}


		public Task BeginStore(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now,ResourceTypeID=ResourceTypeIDs.Stone };
			TaskCreated?.Invoke(this, new Task[] { task });
			return task;
		}

		

		public void EndStore(int WorkerID,  ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		
	}
}
