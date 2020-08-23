using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class ProductTable
	{
		public static readonly Column<ProductTable, int> ProductID = new Column<ProductTable, int>() { IsIdentity=true, IsPrimaryKey = true };
		public static readonly Column<ProductTable, int> FactoryTypeID = new Column<ProductTable, int>();
		public static readonly Column<ProductTable, int> ResourceTypeID = new Column<ProductTable, int>();
		public static readonly Column<ProductTable, int> Quantity = new Column<ProductTable, int>();
		public static readonly Column<ProductTable, int> Duration = new Column<ProductTable, int>();
	}
}
