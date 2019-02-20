using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Task
	{
		public static readonly Column<Task, DbInt> TaskID = new Column<Task, DbInt>() { IsPrimaryKey = true };
		//public static readonly Column<Task, DbInt> FactoryID = new Column<Task, DbInt>() ;
		public static readonly Column<Task, DbString> Name = new Column<Task, DbString>() ;
	}
}
