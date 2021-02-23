using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedHarvesterModule :MockedFunctionalModule,  IHarvesterModule
	{


		public MockedHarvesterModule(bool ThrowException) : base( ThrowException)
		{
		}

		public event TaskCreatedHandler TaskCreated;

		public Task BeginHarvest(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Task task= new Task() { WorkerID = WorkerID, ETA = DateTime.Now };
			TaskCreated?.Invoke(this, new Task[] { task });
			return task;
		}

		public void EndHarvest(int WorkerID)
		{
			throw new NotImplementedException();
		}
	}
}
