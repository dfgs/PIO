using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedWorkerModule :MockedDatabaseModule<Worker>, IWorkerModule
	{

		public MockedWorkerModule(bool ThrowException, params Worker[] Items):base(ThrowException, Items)
		{
		}

		public Worker[] GetWorkers(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.FactoryID == FactoryID).ToArray();
		}

		public Worker GetWorker(int WorkerID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.WorkerID == WorkerID);
		}

		public void UpdateWorker(int WorkerID, int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			Worker item;
			item = GetWorker(WorkerID);
			item.FactoryID = FactoryID;
		}
	}
}
