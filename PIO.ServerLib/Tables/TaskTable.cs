using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class TaskTable
	{
		public static readonly Column<TaskTable, int> TaskID = new Column<TaskTable, int>() { IsPrimaryKey = true };
		//public static readonly Column<Task, int> FactoryID = new Column<Task, int>() ;
		public static readonly Column<TaskTable, string> Name = new Column<TaskTable, string>() ;
	}
}
