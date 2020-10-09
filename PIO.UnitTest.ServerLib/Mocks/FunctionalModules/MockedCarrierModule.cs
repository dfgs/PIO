using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedCarrierModule : ICarrierModule
	{
		public event TaskCreatedHandler TaskCreated;

		public Task BeginCarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
			Task task = new Models.Task() { WorkerID = WorkerID,TargetFactoryID=TargetFactoryID,ResourceTypeID=ResourceTypeID };
			TaskCreated(this, task);
			return task;
		}

		
		public void EndCarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
		}

		
	}
}
