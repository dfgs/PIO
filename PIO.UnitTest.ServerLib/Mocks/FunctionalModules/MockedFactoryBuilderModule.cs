using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedFactoryBuilderModule : IFactoryBuilderModule
	{
		public event TaskCreatedHandler TaskCreated;

		public Task BeginCreateBuilding(int WorkerID, int PlanetID, FactoryTypeIDs FactoryTypeID)
		{
			Task task = new Models.Task() { WorkerID = WorkerID ,FactoryTypeID=FactoryTypeID};
			TaskCreated(this, task);
			return task;
		}

		
		public void Build(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public void EndCreateBuilding(int PlanetID, FactoryTypeIDs FactoryTypeID)
		{
		}

		
	}
}
