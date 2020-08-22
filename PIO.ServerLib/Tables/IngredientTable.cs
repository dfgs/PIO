using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class IngredientTable
	{
		public static readonly Column<IngredientTable, int> IngredientID = new Column<IngredientTable, int>() { IsIdentity=true, IsPrimaryKey = true };
		public static readonly Column<IngredientTable, int> FactoryTypeID = new Column<IngredientTable, int>();
		public static readonly Column<IngredientTable, int> ResourceTypeID = new Column<IngredientTable, int>();
		public static readonly Column<IngredientTable, int> Quantity = new Column<IngredientTable, int>() ;
	}
}
