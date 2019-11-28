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

namespace PIO.ServerLib.Modules
{
	public class ScheduledTaskModule : DatabaseModule,IScheduledTaskModule
	{

		public ScheduledTaskModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row<ScheduledTask> GetScheduledTask(int ScheduledTaskID)
		{
			ISelect<ScheduledTask> query;
			LogEnter();

			query = new Select<ScheduledTask>(ScheduledTask.ScheduledTaskID, ScheduledTask.FactoryID, ScheduledTask.TaskID, ScheduledTask.ETA).Where(ScheduledTask.ScheduledTaskID.IsEqualTo(ScheduledTaskID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}
		public void DeleteScheduledTask(int ScheduledTaskID)
		{
			IDelete query;
			LogEnter();

			query = new Delete<ScheduledTask>().Where(ScheduledTask.ScheduledTaskID.IsEqualTo(ScheduledTaskID));
			Try(query).OrThrow("Failed to query");
		}
		public IEnumerable<Row<ScheduledTask>> GetScheduledTasks(int FactoryID)
		{
			ISelect<ScheduledTask> query;
			LogEnter();

			query = new Select<ScheduledTask>(ScheduledTask.ScheduledTaskID, ScheduledTask.FactoryID, ScheduledTask.TaskID, ScheduledTask.ETA).Where(ScheduledTask.FactoryID.IsEqualTo(FactoryID));
			return Try(query).OrThrow("Failed to query");
		}
		public int CreateScheduledTask(int FactoryID, int TaskID,DateTime ETA)
		{
			IQuery[] queries;
			int result = -1;
			LogEnter();

			queries = new IQuery[] { new Insert<ScheduledTask>().Set(ScheduledTask.FactoryID, FactoryID).Set(ScheduledTask.TaskID, TaskID).Set(ScheduledTask.ETA,ETA), new SelectIdentity<ScheduledTask>((key) => result = Convert.ToInt32(key)) };
			Try(queries).OrThrow("Failed to query");

			return result;
		}


		/*public void SetScheduledTask(int FactoryID, int ScheduledTaskID)
		{
			IQuery[] queries;
			LogEnter();

			queries = new IQuery[] { new Delete<ScheduledTask>().Where(ScheduledTask.FactoryID.IsEqualTo(FactoryID)), new Insert<ScheduledTask>().Set(ScheduledTask.FactoryID,FactoryID).Set(ScheduledTask.Name,"test").Set(ScheduledTask.ScheduledTaskID,ScheduledTaskID) };

			Try(queries).OrThrow("Failed to query");

		}*/




	}
}
