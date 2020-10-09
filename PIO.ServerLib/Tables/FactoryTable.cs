using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class FactoryTable : Table
	{
		public static readonly Column<int> FactoryID = new Column<int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<int> PlanetID = new Column<int>();
		public static readonly Column<FactoryTypeIDs> FactoryTypeID = new Column<FactoryTypeIDs>();
		public static readonly Column<int> HealthPoints = new Column<int>();


	}
}
