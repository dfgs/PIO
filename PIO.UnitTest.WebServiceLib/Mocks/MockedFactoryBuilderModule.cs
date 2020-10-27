using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedFactoryBuilderModule :MockedFunctionalModule,  IFactoryBuilderModule
	{

		public event TaskCreatedHandler TaskCreated;

		public MockedFactoryBuilderModule(bool ThrowException) : base( ThrowException)
		{
		}



		public Task BeginCreateBuilding(int WorkerID,FactoryTypeIDs FactoryTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now, FactoryTypeID=FactoryTypeID };
			TaskCreated?.Invoke(this, task);
			return task;
		}

		public void EndCreateBuilding(int PlanetID, FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Task BeginBuild(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, task);
			return task;
		}

		public void EndBuild(int WorkerID)
		{
			throw new NotImplementedException();
		}

	}
}
