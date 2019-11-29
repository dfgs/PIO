using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class TaskTypeTable
	{
		public static readonly Column<TaskTypeTable, int> TaskTypeID = new Column<TaskTypeTable, int>() { IsPrimaryKey = true };
		//public static readonly Column<Task, int> FactoryID = new Column<Task, int>() ;
		public static readonly Column<TaskTypeTable, string> Name = new Column<TaskTypeTable, string>() ;
	}
}
