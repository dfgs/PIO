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
using PIO.Models.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class WorkerModule : DatabaseModule,IWorkerModule
	{

		public WorkerModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Worker GetWorker(int WorkerID)
		{
			ISelect<WorkerTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Worker table (WorkerID={WorkerID})");
			query = new Select<WorkerTable>(WorkerTable.WorkerID,WorkerTable.FactoryID).Where(WorkerTable.WorkerID.IsEqualTo(WorkerID));
			return TrySelectFirst<WorkerTable,Worker>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Worker[] GetWorkers(int FactoryID)
		{
			ISelect<WorkerTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Worker table (FactoryID={FactoryID})");
			query = new Select<WorkerTable>( WorkerTable.WorkerID, WorkerTable.FactoryID).Where(WorkerTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<WorkerTable, Worker>(query).OrThrow<PIODataException>("Failed to query");
		}

		


	}
}
