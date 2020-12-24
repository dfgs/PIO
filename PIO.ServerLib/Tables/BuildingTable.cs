using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class BuildingTable : Table
	{
		public static readonly Column<int> BuildingID = new Column<int>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey, IsIdentity = true };
		public static readonly Column<int> PlanetID = new Column<int>();
		public static readonly Column<int> X = new Column<int>();
		public static readonly Column<int> Y = new Column<int>();
		public static readonly Column<int> HealthPoints = new Column<int>();
		public static readonly Column<int> RemainingBuildSteps = new Column<int>();


	}
}
