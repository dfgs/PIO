using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Modules
{
	public class TaskModule : DatabaseModule,ITaskModule
	{

		public TaskModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		public Task GetTask(int TaskID)
		{
			ISelect<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table (TaskID={TaskID})");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA, TaskTable.TargetFactoryID, TaskTable.TargetResourceTypeID).Where(TaskTable.TaskID.IsEqualTo(TaskID));
			return TrySelectFirst<TaskTable, Task>(query).OrThrow("Failed to query");
		}

		public void RemoveTask(int TaskID)
		{
			IDelete<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Deleting from Task table (TaskID={TaskID})");
			query = new Delete<TaskTable>().Where(TaskTable.TaskID.IsEqualTo(TaskID));
			Try(query).OrThrow("Failed to delete");
		}

		public Task[] GetTasks(int FactoryID)
		{
			ISelect<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table (FactoryID={FactoryID})");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA, TaskTable.TargetFactoryID, TaskTable.TargetResourceTypeID).Where(TaskTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<TaskTable, Task>(query).OrThrow("Failed to query");
		}

		public Task[] GetTasks()
		{
			ISelect<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA, TaskTable.TargetFactoryID, TaskTable.TargetResourceTypeID);
			return TrySelectMany<TaskTable, Task>(query).OrThrow("Failed to query");
		}

		public Task CreateTask(int FactoryID, int TaskTypeID,int? TargetResourceTypeID, DateTime ETA)
		{
			IInsert<TaskTable> query;
			Task item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into Task table (FactoryID={FactoryID}, TaskTypeID={TaskTypeID}, TargetResourceTypeID={TargetResourceTypeID}, ETA={ETA})");
			item = new Task() { FactoryID=FactoryID,TaskTypeID=TaskTypeID,TargetResourceTypeID=TargetResourceTypeID, ETA=ETA };
			query = new Insert<TaskTable>().Set(TaskTable.FactoryID,item.FactoryID).Set(TaskTable.TaskTypeID, item.TaskTypeID).Set(TaskTable.TargetResourceTypeID, item.TargetResourceTypeID).Set(TaskTable.ETA,item.ETA);
			result=Try(query).OrThrow("Failed to insert");
			item.TaskID = Convert.ToInt32(result);
			return item;
		}



	}
}
