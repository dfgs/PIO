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



		public Task BeginCreateBuilding(int WorkerID, int PlanetID, FactoryTypeIDs FactoryTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task = new Task() { WorkerID = WorkerID, ETA = DateTime.Now, PlanetID=PlanetID,FactoryTypeID=FactoryTypeID };
			TaskCreated?.Invoke(this, task);
			return task;
		}

		public void EndCreateBuilding(int PlanetID, FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public void Build(int FactoryID)
		{
			throw new NotImplementedException();
		}
	}
}
