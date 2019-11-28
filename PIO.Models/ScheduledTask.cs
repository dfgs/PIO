using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.Models
{
	public class ScheduledTask
	{
		public static readonly Column<ScheduledTask, DbInt> ScheduledTaskID = new Column<ScheduledTask, DbInt>() { IsIdentity=true, IsPrimaryKey = true };
		public static readonly Column<ScheduledTask, DbInt> FactoryID = new Column<ScheduledTask, DbInt>();
		public static readonly Column<ScheduledTask, DbInt> TaskID = new Column<ScheduledTask, DbInt>();
		public static readonly Column<ScheduledTask, DbDate> ETA = new Column<ScheduledTask, DbDate>() ;
	}
}
