using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class FactoryTable
	{
		public static readonly Column<FactoryTable, int> FactoryID = new Column<FactoryTable, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<FactoryTable, int> PlanetID = new Column<FactoryTable, int>();
		public static readonly Column<FactoryTable, string> Name = new Column<FactoryTable, string>() {DefaultValue="New factory" };
		public static readonly Column<FactoryTable, int> StateID = new Column<FactoryTable, int>() { DefaultValue = 0 };
		

	}
}
