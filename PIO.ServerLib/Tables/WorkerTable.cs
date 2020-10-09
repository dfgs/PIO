using NetORMLib.Columns;
using NetORMLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class WorkerTable:Table
	{
		public static readonly Column<int> WorkerID = new Column<int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<int> FactoryID = new Column<int>();


	}
}
