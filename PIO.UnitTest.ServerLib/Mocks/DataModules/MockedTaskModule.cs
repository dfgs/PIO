using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedTaskModule :MockedDatabaseModule<Task>,ITaskModule
	{

		public MockedTaskModule(bool ThrowException,params Task[] Items ):base(ThrowException,Items)
		{
		}

		public Task GetTask(int TaskID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException",null,1,"UnitTest","UnitTest");
			return items.FirstOrDefault(item => item.TaskID == TaskID);
		}
		public void DeleteTask(int TaskID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task= items.FirstOrDefault(item => item.TaskID == TaskID);
			items.Remove(task);
		}
		public Task[] GetTasks(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item=>item.WorkerID==WorkerID).ToArray();
		}

		public Task[] GetTasks()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.ToArray();
		}

		public Task GetLastTask(int WorkerID)
		{

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.LastOrDefault();
		}
		public Task CreateTask(TaskTypeIDs TaskTypeID, int WorkerID, int? X, int? Y, int? BuildingID,  ResourceTypeIDs? ResourceTypeID, FactoryTypeIDs? FactoryTypeID, DateTime ETA)
		{
			Task item;

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");

			item=new Task() { TaskTypeID = TaskTypeID,  WorkerID=WorkerID,BuildingID=BuildingID,  ResourceTypeID=ResourceTypeID,FactoryTypeID=FactoryTypeID, ETA=ETA };
			items.Add(item);
			return item;
		}


	}
}
