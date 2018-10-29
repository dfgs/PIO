using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	// No identity for enum tables
	public class Resource
	{
		public static readonly Column<Resource, int> ResourceID = new Column<Resource, int>() { IsPrimaryKey = true};
		public static readonly Column<Resource, string> Name = new Column<Resource, string>() ;
	}
}
