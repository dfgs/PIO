using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class ResourceTypeTable
	{
		public static readonly Column<ResourceTypeTable, int> ResourceTypeID = new Column<ResourceTypeTable, int>() { IsPrimaryKey = true};
		public static readonly Column<ResourceTypeTable, string> Name = new Column<ResourceTypeTable, string>() ;
	}
}
