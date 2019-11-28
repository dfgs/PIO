using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Modules
{
	public class ScheduledTaskModule : DatabaseModule,IScheduledTaskModule
	{

		public ScheduledTaskModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public ScheduledTask GetScheduledTask(int ScheduledTaskID)
		{
			ISelect<ScheduledTaskTable> query;
			LogEnter();

			query = new Select<ScheduledTaskTable>(ScheduledTaskTable.ScheduledTaskID, ScheduledTaskTable.FactoryID, ScheduledTaskTable.TaskID, ScheduledTaskTable.ETA).Where(ScheduledTaskTable.ScheduledTaskID.IsEqualTo(ScheduledTaskID));
			return Try<ScheduledTaskTable,ScheduledTask>(query).OrThrow("Failed to query").FirstOrDefault();
		}
		public void DeleteScheduledTask(int ScheduledTaskID)
		{
			IDelete query;
			LogEnter();

			query = new Delete<ScheduledTaskTable>().Where(ScheduledTaskTable.ScheduledTaskID.IsEqualTo(ScheduledTaskID));
			Try(query).OrThrow("Failed to query");
		}
		public IEnumerable<ScheduledTask> GetScheduledTasks(int FactoryID)
		{
			ISelect<ScheduledTaskTable> query;
			LogEnter();

			query = new Select<ScheduledTaskTable>(ScheduledTaskTable.ScheduledTaskID, ScheduledTaskTable.FactoryID, ScheduledTaskTable.TaskID, ScheduledTaskTable.ETA).Where(ScheduledTaskTable.FactoryID.IsEqualTo(FactoryID));
			return Try<ScheduledTaskTable, ScheduledTask>(query).OrThrow("Failed to query");
		}
		public int CreateScheduledTask(int FactoryID, int TaskID,DateTime ETA)
		{
			IQuery[] queries;
			int result = -1;
			LogEnter();

			queries = new IQuery[] { new Insert<ScheduledTaskTable>().Set(ScheduledTaskTable.FactoryID, FactoryID).Set(ScheduledTaskTable.TaskID, TaskID).Set(ScheduledTaskTable.ETA,ETA), new SelectIdentity<ScheduledTask>((key) => result = Convert.ToInt32(key)) };
			Try(queries).OrThrow("Failed to query");

			return result;
		}


		/*public void SetScheduledTask(int FactoryID, int ScheduledTaskID)
		{
			IQuery[] queries;
			LogEnter();

			queries = new IQuery[] { new Delete<ScheduledTask>().Where(ScheduledTaskTable.FactoryID.IsEqualTo(FactoryID)), new Insert<ScheduledTask>().Set(ScheduledTaskTable.FactoryID,FactoryID).Set(ScheduledTaskTable.Name,"test").Set(ScheduledTaskTable.ScheduledTaskID,ScheduledTaskID) };

			Try(queries).OrThrow("Failed to query");

		}*/




	}
}
