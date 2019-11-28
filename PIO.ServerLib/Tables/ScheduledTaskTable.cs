using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class ScheduledTaskTable
	{
		public static readonly Column<ScheduledTaskTable, int> ScheduledTaskID = new Column<ScheduledTaskTable, int>() { IsIdentity=true, IsPrimaryKey = true };
		public static readonly Column<ScheduledTaskTable, int> FactoryID = new Column<ScheduledTaskTable, int>();
		public static readonly Column<ScheduledTaskTable, int> TaskID = new Column<ScheduledTaskTable, int>();
		public static readonly Column<ScheduledTaskTable, DateTime> ETA = new Column<ScheduledTaskTable, DateTime>() ;
	}
}
