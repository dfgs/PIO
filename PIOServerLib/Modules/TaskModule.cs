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

namespace PIOServerLib.Modules
{
	public class TaskModule : DatabaseModule,ITaskModule
	{

		public TaskModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row<Task> GetTask(int TaskID)
		{
			ISelect<Task> query;
			LogEnter();

			query = new Select<Task>(Task.TaskID, Task.Name).Where(Task.TaskID.IsEqualTo(TaskID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

		/*public IEnumerable<Row> GetTasks(int FactoryID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Task>(Task.TaskID, Task.Name).Where(Task.FactoryID.IsEqualTo(FactoryID));
			return Try(query).OrThrow("Failed to query");
		}*/

		/*public void SetTask(int FactoryID, int TaskID)
		{
			IQuery[] queries;
			LogEnter();

			queries = new IQuery[] { new Delete<Task>().Where(Task.FactoryID.IsEqualTo(FactoryID)), new Insert<Task>().Set(Task.FactoryID,FactoryID).Set(Task.Name,"test").Set(Task.TaskID,TaskID) };

			Try(queries).OrThrow("Failed to query");

		}*/




	}
}
