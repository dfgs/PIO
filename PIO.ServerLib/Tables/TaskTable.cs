using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class TaskTable
	{
		public static readonly Column<TaskTable, int> TaskID = new Column<TaskTable, int>() { IsIdentity=true, IsPrimaryKey = true };
		public static readonly Column<TaskTable, int> FactoryID = new Column<TaskTable, int>();
		public static readonly Column<TaskTable, int> TaskTypeID = new Column<TaskTable, int>();
		public static readonly Column<TaskTable, DateTime> ETA = new Column<TaskTable, DateTime>() ;
		public static readonly Column<TaskTable, int> TargetFactoryID = new Column<TaskTable, int>() { IsNullable = true };
		public static readonly Column<TaskTable, int> TargetResourceTypeID = new Column<TaskTable, int>() { IsNullable = true };
	}
}
