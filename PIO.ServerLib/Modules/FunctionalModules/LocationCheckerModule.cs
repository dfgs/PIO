using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using PIOBaseModulesLib.Modules.FunctionalModules;


namespace PIO.ServerLib.Modules
{
	public class LocationCheckerModule : FunctionalModule, ILocationCheckerModule
	{
		private IWorkerModule workerModule;
		private IBuildingModule buildingModule;

		public LocationCheckerModule(ILogger Logger,IWorkerModule WorkerModule,IBuildingModule BuildingModule ) : base(Logger)
		{
			this.workerModule = WorkerModule;  this.buildingModule = BuildingModule;
		}

		public bool WorkerIsInBuilding(int WorkerID, int BuildingID)
		{
			Building building;
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");
			building=AssertExists(() => buildingModule.GetBuilding(BuildingID), $"BuildingID={BuildingID}");


			Log(LogLevels.Information, "Checking is worker is in building");
			if (worker.PlanetID != building.PlanetID)
			{
				Log(LogLevels.Information, "Worker and Building are not in same Planet");
				return false;
			}
			if ((worker.X != building.X) || (worker.Y != building.Y))
			{
				Log(LogLevels.Information, "Worker is not in building");
				return false;
			}

			Log(LogLevels.Information, "Worker is in building");
			return true;
		}

	}
}
