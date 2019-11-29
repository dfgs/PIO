using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class StackTable
	{
		public static readonly Column<StackTable, int> StackID = new Column<StackTable, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<StackTable, int> FactoryID = new Column<StackTable, int>();
		public static readonly Column<StackTable, int> ResourceTypeID = new Column<StackTable, int>();
		public static readonly Column<StackTable, int> Quantity = new Column<StackTable, int>() {DefaultValue=0 } ;
	}
}
