using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedWorkerModule :MockedDatabaseModule<Worker>, IWorkerModule
	{
		public ResourceTypeIDs? ResourceTypeID;

		public MockedWorkerModule(bool ThrowException, params Worker[] Items):base(ThrowException, Items)
		{
		}

		public Worker[] GetWorkers(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.PlanetID == PlanetID).ToArray();
		}
		public Worker[] GetWorkers()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.ToArray();
		}

		public Worker GetWorker(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.WorkerID == WorkerID);
		}

		public void UpdateWorker(int WorkerID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Worker item;
			item = GetWorker(WorkerID);
			item.X = X;
			item.Y = Y;
		}

		public void UpdateWorker(int WorkerID, ResourceTypeIDs? ResourceTypeID)
		{
			this.ResourceTypeID = ResourceTypeID;
		}

		public Worker CreateWorker(int PlanetID, int X, int Y)
		{
			throw new NotImplementedException();
		}
	}
}
