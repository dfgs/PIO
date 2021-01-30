using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class WorkerTable:Table
	{
		public static readonly Column<int> WorkerID = new Column<int>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey, IsIdentity = true };
		public static readonly Column<int> PlanetID = new Column<int>();
		public static readonly Column<int> X = new Column<int>();
		public static readonly Column<int> Y = new Column<int>();
		public static readonly Column<ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeIDs>() { IsNullable=true};


	}
}
