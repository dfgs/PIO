using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedStorerModule : IStorerModule
	{
		public event TaskCreatedHandler TaskCreated;

		public Task BeginStore(int WorkerID)
		{
			Task task = new Models.Task() { WorkerID = WorkerID,ResourceTypeID=ResourceTypeIDs.Stone };
			TaskCreated(this, new Task[] { task });
			return task;
		}


		public void EndStore(int WorkerID, ResourceTypeIDs ResourceTypeID)
		{
		}

		
	}
}
