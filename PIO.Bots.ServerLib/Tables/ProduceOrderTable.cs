﻿using NetORMLib.Columns;
using NetORMLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.ServerLib.Tables
{
	public class ProduceOrderTable:Table
	{
		public static readonly Column<int> ProduceOrderID = new Column<int>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey, IsIdentity = true };
		public static readonly Column<int> OrderID = new Column<int>();
		// planet ID cannot be part of order table because of constraint
		public static readonly Column<int> PlanetID = new Column<int>();
		public static readonly Column<int> BuildingID = new Column<int>();
	}
}
