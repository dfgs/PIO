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

		public Task[] GetTasks(int FactoryID)
		{
			Task[] items;

			if (throwException) throw new NotImplementedException();
			items = new Task[results];
			for (int t = 0; t < results; t++)
			{
				items[t]=new Task() { TaskID = t, FactoryID = FactoryID, ETA = DateTime.Now, TaskTypeID = 1 };
			}
			return items;
		}

		public Task[] GetTasks()
		{
			Task[] items;

			if (throwException) throw new NotImplementedException();
			items = new Task[results];
			for (int t = 0; t < results; t++)
			{
				items[t] = new Task() { TaskID = t, FactoryID = 1, ETA = DateTime.Now, TaskTypeID = 1 };
			}
			return items;
		}
	}
}
