using PIO.ServerLib.TaskHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedTaskHandler : ITaskHandler
	{
		private int taskTypeID;
		public int TaskTypeID => taskTypeID;


		public MockedTaskHandler(int TaskTypeID)
		{
			this.taskTypeID = TaskTypeID;
		}
	}
}
