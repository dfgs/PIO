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
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class TaskModule : DatabaseModule,ITaskModule
	{

		public TaskModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}


		public Task GetTask(int TaskID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying Task table (TaskID={TaskID})");
			query=new Select(TaskTable.TaskID, TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.X, TaskTable.Y, TaskTable.BuildingID,  TaskTable.ResourceTypeID, TaskTable.FactoryTypeID, TaskTable.FarmTypeID, TaskTable.ETA).From(PIODB.TaskTable).Where(TaskTable.TaskID.IsEqualTo(TaskID));
			return TrySelectFirst<TaskTable, Task>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Task GetLastTask(int WorkerID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table (WorkerID={WorkerID})");
			query=new Select(TaskTable.TaskID, TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.X, TaskTable.Y, TaskTable.BuildingID,  TaskTable.ResourceTypeID, TaskTable.FactoryTypeID, TaskTable.FarmTypeID, TaskTable.ETA).Top(1).From(PIODB.TaskTable).Where(TaskTable.WorkerID.IsEqualTo(WorkerID)).OrderBy(OrderModes.DESC, TaskTable.TaskID);
			return TrySelectFirst<TaskTable, Task>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Task[] GetTasks(int WorkerID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table (WorkerID={WorkerID})");
			query=new Select(TaskTable.TaskID, TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.X, TaskTable.Y, TaskTable.BuildingID, TaskTable.ResourceTypeID, TaskTable.FactoryTypeID, TaskTable.FarmTypeID, TaskTable.ETA).From(PIODB.TaskTable).Where(TaskTable.WorkerID.IsEqualTo(WorkerID));
			return TrySelectMany<TaskTable, Task>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Task[] GetTasks()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table");
			query=new Select(TaskTable.TaskID, TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.X, TaskTable.Y, TaskTable.BuildingID,  TaskTable.ResourceTypeID, TaskTable.FactoryTypeID, TaskTable.FarmTypeID, TaskTable.ETA).From(PIODB.TaskTable);
			return TrySelectMany<TaskTable, Task>(query).OrThrow<PIODataException>("Failed to query");
		}


		public Task CreateTask(TaskTypeIDs TaskTypeID, int WorkerID, int X, int Y,int? BuildingID, ResourceTypeIDs? ResourceTypeID, FactoryTypeIDs? FactoryTypeID,FarmTypeIDs? FarmTypeID, DateTime ETA)
		{
			IInsert query;
			Task item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into Task table (TaskTypeID={TaskTypeID}, WorkerID={WorkerID}, ETA={ETA})");
			item = new Task() { TaskTypeID = TaskTypeID, WorkerID = WorkerID, X = X, Y = Y, BuildingID = BuildingID, ResourceTypeID = ResourceTypeID, FactoryTypeID = FactoryTypeID, FarmTypeID = FarmTypeID, ETA=ETA };
			query = new Insert().Into(PIODB.TaskTable).Set(TaskTable.TaskTypeID,item.TaskTypeID).Set(TaskTable.WorkerID, item.WorkerID)
				.Set(TaskTable.X,item.X).Set(TaskTable.Y,item.Y)
				.Set(TaskTable.BuildingID, item.BuildingID)
				.Set(TaskTable.ResourceTypeID, item.ResourceTypeID)
				.Set(TaskTable.FactoryTypeID, item.FactoryTypeID)
				.Set(TaskTable.FarmTypeID, item.FarmTypeID)
				.Set(TaskTable.ETA,item.ETA);
			result=Try(query).OrThrow<PIODataException>("Failed to insert");
			item.TaskID=Convert.ToInt32(result);
			return item;
		}

		public void DeleteTask(int TaskID)
		{
			IDelete query;

			LogEnter();
			Log(LogLevels.Information, $"Deleting from Task table (TaskID={TaskID})");
			query=new Delete().From(PIODB.TaskTable).Where(TaskTable.TaskID.IsEqualTo(TaskID));
			Try(query).OrThrow<PIODataException>("Failed to delete");
		}

	}
}
