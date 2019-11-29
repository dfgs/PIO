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
	public class TaskModule : DatabaseModule,ITaskModule
	{

		public TaskModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Task GetTask(int TaskID)
		{
			ISelect<TaskTable> query;
			LogEnter();

			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.Name).Where(TaskTable.TaskID.IsEqualTo(TaskID));
			return TrySelectMany<TaskTable,Task>(query).OrThrow("Failed to query").FirstOrDefault();
		}

		/*public IEnumerable<Row> GetTasks(int FactoryID)
		{
			ISelect query;
			LogEnter();

			query = new Select<TaskTable>(TaskTable.TaskID, TaskTable.Name).Where(TaskTable.FactoryID.IsEqualTo(FactoryID));
			return Try(query).OrThrow("Failed to query");
		}*/

		/*public void SetTask(int FactoryID, int TaskID)
		{
			IQuery[] queries;
			LogEnter();

			queries = new IQuery[] { new Delete<Task>().Where(TaskTable.FactoryID.IsEqualTo(FactoryID)), new Insert<Task>().Set(TaskTable.FactoryID,FactoryID).Set(TaskTable.Name,"test").Set(TaskTable.TaskID,TaskID) };

			Try(queries).OrThrow("Failed to query");

		}*/




	}
}
