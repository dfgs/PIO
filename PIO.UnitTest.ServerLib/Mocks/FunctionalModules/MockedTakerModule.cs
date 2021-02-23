using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedTakerModule : ITakerModule
	{
		public event TaskCreatedHandler TaskCreated;

		public Task BeginTake(int WorkerID, ResourceTypeIDs ResourceTypeID)
		{
			Task task = new Models.Task() { WorkerID = WorkerID,ResourceTypeID=ResourceTypeID };
			TaskCreated(this, new Task[] { task });
			return task;
		}


		public void EndTake(int WorkerID, ResourceTypeIDs ResourceTypeID)
		{
		}

		
	}
}
