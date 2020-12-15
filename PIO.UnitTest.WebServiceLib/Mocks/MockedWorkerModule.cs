using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedWorkerModule : MockedDatabaseModule, IWorkerModule
	{
		public MockedWorkerModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public Worker GetWorker(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Worker() { WorkerID = WorkerID };
		}

		public Worker[] GetWorkers(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Worker() { WorkerID = t, PlanetID = PlanetID});
		}

		public void UpdateWorker(int WorkerID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
		}
	}
}
