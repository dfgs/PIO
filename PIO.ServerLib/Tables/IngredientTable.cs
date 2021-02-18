using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class IngredientTable : Table
	{
		public static readonly Column<int> IngredientID = new Column<int>() { IsIdentity=true, Constraint = NetORMLib.ColumnConstraints.PrimaryKey };
		public static readonly Column<BuildingTypeIDs> BuildingTypeID = new Column<BuildingTypeIDs>();
		public static readonly Column<ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeIDs>();
		public static readonly Column<int> Quantity = new Column<int>() ;
	}
}
