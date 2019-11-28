using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.Models
{
	public class Task
	{
		public static readonly Column<Task, DbInt> TaskID = new Column<Task, DbInt>() { IsPrimaryKey = true };
		//public static readonly Column<Task, int> FactoryID = new Column<Task, int>() ;
		public static readonly Column<Task, DbString> Name = new Column<Task, DbString>() ;
	}
}
