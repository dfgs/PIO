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
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public class TaskTypeModule : DatabaseModule,ITaskTypeModule
	{

		public TaskTypeModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public TaskType GetTaskType(int TaskTypeID)
		{
			ISelect<TaskTypeTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying task type with TaskTypeID {TaskTypeID}");
			query = new Select<TaskTypeTable>(TaskTypeTable.TaskTypeID, TaskTypeTable.Name).Where(TaskTypeTable.TaskTypeID.IsEqualTo(TaskTypeID));
			return TrySelectFirst<TaskTypeTable,TaskType>(query).OrThrow("Failed to query");
		}

		public TaskType[] GetTaskTypes()
		{
			ISelect<TaskTypeTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying task types");
			query = new Select<TaskTypeTable>(TaskTypeTable.TaskTypeID, TaskTypeTable.Name);
			return TrySelectMany<TaskTypeTable,TaskType>(query).OrThrow("Failed to query");
		}

	}
}
