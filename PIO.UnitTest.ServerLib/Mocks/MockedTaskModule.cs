using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedTaskModule :ITaskModule
	{
		private bool throwException;
		private int results;

		public MockedTaskModule(bool ThrowException,int Results )
		{
			this.throwException = ThrowException;
			this.results = Results;
		}

		public Task GetTask(int TaskID)
		{
			if (throwException) throw new NotImplementedException();
			return new Task() {TaskID=TaskID ,FactoryID=1,ETA=DateTime.Now,TaskTypeID=1};
		}

		public IEnumerable<Task> GetTasks(int FactoryID)
		{
			if (throwException) throw new NotImplementedException();
			for (int t = 0; t < results; t++)
			{
				yield return new Task() { TaskID = t, FactoryID = FactoryID, ETA = DateTime.Now, TaskTypeID = 1 };
			}
		}

		public IEnumerable<Task> GetTasks()
		{
			if (throwException) throw new NotImplementedException();
			for (int t = 0; t < results; t++)
			{
				yield return new Task() { TaskID = t, FactoryID = 1, ETA = DateTime.Now, TaskTypeID = 1 };
			}
		}
	}
}
