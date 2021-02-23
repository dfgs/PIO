using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedFactoryBuilderModule : IBuilderModule
	{
		public event TaskCreatedHandler TaskCreated;

		public Task BeginCreateBuilding(int WorkerID, BuildingTypeIDs BuildingTypeID)
		{
			Task task = new Models.Task() { WorkerID = WorkerID , BuildingTypeID = BuildingTypeID };
			TaskCreated(this, new Task[] { task });
			return task;
		}

		
		

		public void EndCreateBuilding(int PlanetID, BuildingTypeIDs BuildingTypeID)
		{
		}

		public Task BeginBuild(int WorkerID)
		{
			Task task = new Models.Task() { WorkerID = WorkerID};
			TaskCreated(this, new Task[] { task });
			return task;
		}

		public void EndBuild(int WorkerID)
		{
			throw new NotImplementedException();
		}
	}
}
