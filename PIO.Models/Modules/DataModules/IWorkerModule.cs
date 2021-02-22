using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface IWorkerModule : IDatabaseModule
	{
		Worker GetWorker(int WorkerID);
		Worker[] GetWorkers();
		Worker[] GetWorkers(int PlanetID);

		Worker CreateWorker(int PlanetID, int X, int Y);

		void UpdateWorker(int WorkerID, int X, int Y);
		void UpdateWorker(int WorkerID, ResourceTypeIDs? ResourceTypeID);
	}
}
