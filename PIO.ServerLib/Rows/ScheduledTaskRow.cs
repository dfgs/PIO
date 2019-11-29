using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Rows
{
	public class ScheduledTaskRow
	{
		public int ScheduledTaskID {get;set;}
		public int FactoryID {get;set;}
		public int TaskID {get;set;}
		public DateTime ETA {get;set;}
	}
}
