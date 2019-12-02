using PIO.Models;
using PIO.ServerLib;
using PIO.ServerLib.TaskHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedTaskHandler : ITaskHandler
	{
		private int taskTypeID;
		public int TaskTypeID => taskTypeID;

		public int ID => 0;

		public string ModuleName => throw new NotImplementedException();

		public MockedTaskHandler(int TaskTypeID)
		{
			this.taskTypeID = TaskTypeID;
		}

		public void Execute(ITaskSchedulerModule TaskSchedulerModule, Task Task)
		{
			throw new NotImplementedException();
		}
	}
}
