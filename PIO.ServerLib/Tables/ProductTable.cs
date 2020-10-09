using NetORMLib.Columns;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class ProductTable
	{
		public static readonly Column<ProductTable, int> ProductID = new Column<ProductTable, int>() { IsIdentity=true, IsPrimaryKey = true };
		public static readonly Column<ProductTable, FactoryTypeIDs> FactoryTypeID = new Column<ProductTable, FactoryTypeIDs>();
		public static readonly Column<ProductTable, ResourceTypeIDs> ResourceTypeID = new Column<ProductTable, ResourceTypeIDs>();
		public static readonly Column<ProductTable, int> Quantity = new Column<ProductTable, int>();
		public static readonly Column<ProductTable, int> Duration = new Column<ProductTable, int>();
	}
}
