using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedTaskTypeModule : MockedDatabaseModule, ITaskTypeModule
	{
		public MockedTaskTypeModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public TaskType GetTaskType(TaskTypeIDs TaskTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new TaskType() { TaskTypeID = TaskTypeID };
		}

		public TaskType[] GetTaskTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new TaskType() { TaskTypeID = (TaskTypeIDs)t });
		}

		
	}
}
