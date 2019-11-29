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

		public Task GetScheduledTask(int ScheduledTaskID)
		{
			ISelect<TaskTable> query;
			LogEnter();

			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA).Where(TaskTable.TaskID.IsEqualTo(ScheduledTaskID));
			return TrySelectMany<TaskTable,Task>(query).OrThrow("Failed to query").FirstOrDefault();
		}
		public void DeleteScheduledTask(int ScheduledTaskID)
		{
			IDelete query;
			LogEnter();

			query = new Delete<TaskTable>().Where(TaskTable.TaskID.IsEqualTo(ScheduledTaskID));
			Try(query).OrThrow("Failed to query");
		}
		public IEnumerable<Task> GetScheduledTasks(int FactoryID)
		{
			ISelect<TaskTable> query;
			LogEnter();

			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA).Where(TaskTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<TaskTable, Task>(query).OrThrow("Failed to query");
		}
		public int CreateScheduledTask(int FactoryID, int TaskID,DateTime ETA)
		{
			IQuery[] queries;
			int result = -1;
			LogEnter();

			queries = new IQuery[] { new Insert<TaskTable>().Set(TaskTable.FactoryID, FactoryID).Set(TaskTable.TaskTypeID, TaskID).Set(TaskTable.ETA,ETA), new SelectIdentity<Task>((key) => result = Convert.ToInt32(key)) };
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
