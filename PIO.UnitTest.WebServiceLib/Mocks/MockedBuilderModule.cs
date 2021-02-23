using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedBuilderModule :MockedFunctionalModule,  IBuilderModule
	{

		public event TaskCreatedHandler TaskCreated;

		public MockedBuilderModule(bool ThrowException) : base( ThrowException)
		{
		}



		public Task BeginCreateBuilding(int WorkerID, BuildingTypeIDs BuildingTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now, BuildingTypeID = BuildingTypeID};
			TaskCreated?.Invoke(this, new Task[] { task });
			return task;
		}

		public void EndCreateBuilding(int PlanetID, BuildingTypeIDs BuildingTypeID)
		{
			throw new NotImplementedException();
		}

		public Task BeginBuild(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, new Task[] { task });
			return task;
		}

		public void EndBuild(int WorkerID)
		{
			throw new NotImplementedException();
		}

	}
}
