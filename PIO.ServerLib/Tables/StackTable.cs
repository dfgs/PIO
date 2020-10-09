using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class StackTable : Table
	{
		public static readonly Column<int> StackID = new Column<int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<int> FactoryID = new Column<int>();
		public static readonly Column<ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeIDs>();
		public static readonly Column<int> Quantity = new Column<int>() {DefaultValue=0 } ;
	}
}
