using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	// No identity for enum tables
	public class Status
	{
		public static readonly Column<Status, int> StatusID = new Column<Status, int>() { IsPrimaryKey = true};
		public static readonly Column<Status, string> Name = new Column<Status, string>() ;
	}
}
