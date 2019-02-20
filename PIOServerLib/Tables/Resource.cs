using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	// No identity for enum tables
	public class Resource
	{
		public static readonly Column<Resource, DbInt> ResourceID = new Column<Resource, DbInt>() { IsPrimaryKey = true};
		public static readonly Column<Resource, DbString> Name = new Column<Resource, DbString>() ;
	}
}
