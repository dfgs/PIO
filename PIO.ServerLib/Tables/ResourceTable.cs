using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class ResourceTable
	{
		public static readonly Column<ResourceTable, int> ResourceID = new Column<ResourceTable, int>() { IsPrimaryKey = true};
		public static readonly Column<ResourceTable, string> Name = new Column<ResourceTable, string>() ;
	}
}
