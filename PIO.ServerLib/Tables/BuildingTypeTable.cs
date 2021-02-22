using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class BuildingTypeTable : Table
	{
		public static readonly Column<BuildingTypeIDs> BuildingTypeID = new Column<BuildingTypeIDs>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey };
		public new static readonly Column<string> Name = new Column<string>();
		public static readonly Column<int> HealthPoints = new Column<int>();
		public static readonly Column<int> BuildSteps = new Column<int>();
		public static readonly Column<bool> IsFactory = new Column<bool>();
		public static readonly Column<bool> IsFarm = new Column<bool>();
	}
}
