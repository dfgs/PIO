using NetORMLib.Columns;
using PIO.Models;
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
		public static readonly Column<FactoryTable, FactoryTypeIDs> FactoryTypeID = new Column<FactoryTable, FactoryTypeIDs>();
		public static readonly Column<FactoryTable, int> HealthPoints = new Column<FactoryTable, int>();


	}
}
