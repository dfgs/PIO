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

			Log(LogLevels.Information, $"Querying task with TaskID {TaskID}");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA, TaskTable.TargetFactoryID, TaskTable.TargetResourceTypeID).Where(TaskTable.TaskID.IsEqualTo(TaskID));
			return TrySelectFirst<TaskTable, Task>(query).OrThrow("Failed to query");
		}

		public void RemoveTask(int TaskID)
		{
			IDelete<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Removing task with TaskID {TaskID}");
			query = new Delete<TaskTable>().Where(TaskTable.TaskID.IsEqualTo(TaskID));
			Try(query).OrThrow("Failed to query");
		}

		public Task[] GetTasks(int FactoryID)
		{
			ISelect<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying tasks with FactoryID {FactoryID}");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA, TaskTable.TargetFactoryID, TaskTable.TargetResourceTypeID).Where(TaskTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<TaskTable, Task>(query).OrThrow("Failed to query");
		}

		public Task[] GetTasks()
		{
			ISelect<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying tasks");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA, TaskTable.TargetFactoryID, TaskTable.TargetResourceTypeID);
			return TrySelectMany<TaskTable, Task>(query).OrThrow("Failed to query");
		}

	}
}
