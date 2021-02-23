using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class ResourceTypeTable : Table
	{
		public static readonly Column<ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeIDs>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey };
		public static readonly Column<string> PhraseID = new Column<string>() ;
	}
}
