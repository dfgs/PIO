using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.Mocks
{
	public class MockedWorkerModule : IWorkerModule
	{

		public Func<int, Worker> GetWorkerDelegate;
		public Action<int, int,int> UpdateWorkerDelegate;

		public Worker GetWorker(int WorkerID)
		{
			if (GetWorkerDelegate == null) throw new NotImplementedException("GetWorkerDelegate");
			return GetWorkerDelegate(WorkerID);
		}

		public Worker[] GetWorkers(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public void UpdateWorker(int WorkerID, int X, int Y)
		{
			if (UpdateWorkerDelegate == null) throw new NotImplementedException("UpdateWorkerDelegate");
			UpdateWorkerDelegate(WorkerID,X,Y);
		}//*/

	}
}
