using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedTaskModule : MockedDatabaseModule, ITaskModule
	{
		public MockedTaskModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public Task GetTask(int TaskID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Task() { TaskID = TaskID };
		}

		public Task[] GetTasks(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Task() { TaskID = t,WorkerID=WorkerID });
		}

		public Task InsertTask(int TaskTypeID,int WorkerID, DateTime ETA)
		{
			throw new NotImplementedException();
		}

		public void DeleteTask(int TaskID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
		}


	}
}
