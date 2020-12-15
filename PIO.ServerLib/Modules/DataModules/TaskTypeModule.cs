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
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class TaskTypeModule : DatabaseModule,ITaskTypeModule
	{

		public TaskTypeModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public TaskType GetTaskType(TaskTypeIDs TaskTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying TaskType table (TaskTypeID={TaskTypeID})");
			query = new Select(TaskTypeTable.TaskTypeID, TaskTypeTable.Name).From(PIODB.TaskTypeTable).Where(TaskTypeTable.TaskTypeID.IsEqualTo(TaskTypeID));
			return TrySelectFirst<TaskTypeTable,TaskType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public TaskType[] GetTaskTypes()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying TaskType table");
			query = new Select(TaskTypeTable.TaskTypeID, TaskTypeTable.Name).From(PIODB.TaskTypeTable);
			return TrySelectMany<TaskTypeTable,TaskType>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
