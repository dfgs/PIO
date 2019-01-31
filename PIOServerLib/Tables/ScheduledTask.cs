using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class ScheduledTask
	{
		public static readonly Column<ScheduledTask, int> ScheduledTaskID = new Column<ScheduledTask, int>() { IsIdentity=true, IsPrimaryKey = true };
		public static readonly Column<ScheduledTask, int> FactoryID = new Column<ScheduledTask, int>();
		public static readonly Column<ScheduledTask, int> TaskID = new Column<ScheduledTask, int>();
		public static readonly Column<ScheduledTask, DateTime> ETA = new Column<ScheduledTask, DateTime>() ;
	}
}
