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
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class WorkerModule : DatabaseModule,IWorkerModule
	{

		public WorkerModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Worker GetWorker(int WorkerID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Worker table (WorkerID={WorkerID})");
			query=new Select(WorkerTable.WorkerID,WorkerTable.PlanetID,WorkerTable.X,WorkerTable.Y,WorkerTable.ResourceTypeID).From(PIODB.WorkerTable).Where(WorkerTable.WorkerID.IsEqualTo(WorkerID));
			return TrySelectFirst<WorkerTable,Worker>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Worker[] GetWorkers(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Worker table (PlanetID={PlanetID})");
			query = new Select(WorkerTable.WorkerID, WorkerTable.PlanetID, WorkerTable.X, WorkerTable.Y, WorkerTable.ResourceTypeID).From(PIODB.WorkerTable).Where(WorkerTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<WorkerTable, Worker>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Worker[] GetWorkers()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Worker table");
			query = new Select(WorkerTable.WorkerID, WorkerTable.PlanetID, WorkerTable.X, WorkerTable.Y, WorkerTable.ResourceTypeID).From(PIODB.WorkerTable);
			return TrySelectMany<WorkerTable, Worker>(query).OrThrow<PIODataException>("Failed to query");
		}


		public Worker CreateWorker(int PlanetID, int X, int Y)
		{
			IInsert queryWorker;
			Worker item;
			object result;

			LogEnter();

			item = new Worker() { PlanetID = PlanetID, X = X, Y = Y};

			Log(LogLevels.Information, $"Inserting into Worker table (PlanetID={PlanetID}, X={X}, Y={Y})");
			queryWorker = new Insert().Into(PIODB.WorkerTable).Set(WorkerTable.PlanetID, item.PlanetID).Set(WorkerTable.X, item.X).Set(WorkerTable.Y, item.Y);
			result = Try(queryWorker).OrThrow<PIODataException>("Failed to insert");
			item.WorkerID = Convert.ToInt32(result);

			return item;
		}


		public void UpdateWorker(int WorkerID, int X, int Y)
		{
			IUpdate update;

			LogEnter();

			Log(LogLevels.Information, $"Updating Worker table (WorkerID={WorkerID}, X={X}, Y={Y})");
			update = new Update(PIODB.WorkerTable).Set(WorkerTable.X, X).Set(WorkerTable.Y, Y).Where(WorkerTable.WorkerID.IsEqualTo(WorkerID));
			Try(update).OrThrow<PIODataException>("Failed to update");
		}
		public void UpdateWorker(int WorkerID, ResourceTypeIDs? ResourceTypeID)
		{
			IUpdate update;

			LogEnter();

			Log(LogLevels.Information, $"Updating Worker table (WorkerID={WorkerID}, ResourceTypeID={ResourceTypeID})");
			update = new Update(PIODB.WorkerTable).Set(WorkerTable.ResourceTypeID, ResourceTypeID).Where(WorkerTable.WorkerID.IsEqualTo(WorkerID));
			Try(update).OrThrow<PIODataException>("Failed to update");
		}


	}
}
