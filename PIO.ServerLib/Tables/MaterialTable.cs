using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class MaterialTable : Table
	{
		public static readonly Column<int> MaterialID = new Column<int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<FactoryTypeIDs> FactoryTypeID = new Column<FactoryTypeIDs>();
		public static readonly Column<ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeIDs>();
		public static readonly Column<int> Quantity = new Column<int>() {DefaultValue=0 } ;
	}
}
