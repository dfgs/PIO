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
using PIO.Models.Exceptions;

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
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.FactoryID, TaskTable.ETA).Where(TaskTable.TaskID.IsEqualTo(TaskID));
			return TrySelectFirst<TaskTable, Task>(query).OrThrow<PIODataException>("Failed to query");
		}

		

		public Task[] GetTasks(int WorkerID)
		{
			ISelect<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table (WorkerID={WorkerID})");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.FactoryID, TaskTable.ETA).Where(TaskTable.WorkerID.IsEqualTo(WorkerID));
			return TrySelectMany<TaskTable, Task>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Task[] GetTasks()
		{
			ISelect<TaskTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Task table");
			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.FactoryID, TaskTable.ETA);
			return TrySelectMany<TaskTable, Task>(query).OrThrow<PIODataException>("Failed to query");
		}


		public Task InsertTask(int TaskTypeID, int WorkerID, int FactoryID,  DateTime ETA)
		{
			IInsert<TaskTable> query;
			Task item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into Task table (TaskTypeID={TaskTypeID}, WorkerID={WorkerID}, FactoryID={FactoryID}, ETA={ETA})");
			item = new Task() { TaskTypeID=TaskTypeID, WorkerID=WorkerID, FactoryID=FactoryID, ETA=ETA };
			query = new Insert<TaskTable>().Set(TaskTable.TaskTypeID,item.TaskTypeID).Set(TaskTable.WorkerID, item.WorkerID).Set(TaskTable.FactoryID, item.FactoryID).Set(TaskTable.ETA,item.ETA);
			result=Try(query).OrThrow<PIODataException>("Failed to insert");
			item.TaskID = Convert.ToInt32(result);
			return item;
		}

		public void DeleteTask(int TaskID)
		{
			IDelete<TaskTable> query;

			LogEnter();
			Log(LogLevels.Information, $"Deleting from Task table (TaskID={TaskID})");
			query = new Delete<TaskTable>().Where(TaskTable.TaskID.IsEqualTo(TaskID));
			Try(query).OrThrow<PIODataException>("Failed to delete");
		}

	}
}
