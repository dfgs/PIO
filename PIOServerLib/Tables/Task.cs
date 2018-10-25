using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Task
	{
		public static readonly Column<Task, int> TaskID = new Column<Task, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Task, int> FactoryID = new Column<Task, int>();
		public static readonly Column<Task, string> Name = new Column<Task, string>() ;
	}
}
