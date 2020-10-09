using NetORMLib.Columns;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class ResourceTypeTable
	{
		public static readonly Column<ResourceTypeTable, ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeTable, ResourceTypeIDs>() { IsPrimaryKey = true};
		public static readonly Column<ResourceTypeTable, string> Name = new Column<ResourceTypeTable, string>() ;
	}
}
