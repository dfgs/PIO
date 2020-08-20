using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class WorkerTable
	{
		public static readonly Column<WorkerTable, int> WorkerID = new Column<WorkerTable, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<WorkerTable, int> PlanetID = new Column<WorkerTable, int>();


	}
}
