using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class MaterialTable
	{
		public static readonly Column<MaterialTable, int> MaterialID = new Column<MaterialTable, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<MaterialTable, int> FactoryTypeID = new Column<MaterialTable, int>();
		public static readonly Column<MaterialTable, int> ResourceTypeID = new Column<MaterialTable, int>();
		public static readonly Column<MaterialTable, int> Quantity = new Column<MaterialTable, int>() {DefaultValue=0 } ;
	}
}
