using PIO.Models;
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
			if (ThrowException) throw new Exception("Mocked exception");
			return new Task() { TaskID = TaskID };
		}

		public Task[] GetTasks(int WorkerID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Task() { TaskID = t,WorkerID=WorkerID });
		}
		
		
	}
}
