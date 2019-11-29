using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class FactoryTypeTable
	{
		public static readonly Column<FactoryTypeTable, int> FactoryTypeID = new Column<FactoryTypeTable, int>() { IsPrimaryKey = true };
		public static readonly Column<FactoryTypeTable, string> Name = new Column<FactoryTypeTable, string>();
		public static readonly Column<FactoryTypeTable, int> HealthPoints = new Column<FactoryTypeTable, int>();
	}
}
