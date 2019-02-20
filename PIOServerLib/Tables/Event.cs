using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Event
	{
		public static readonly Column<Event, DbInt> EventID = new Column<Event, DbInt>() { IsPrimaryKey = true, IsIdentity = false };
		public static readonly Column<Event, DbString> Name = new Column<Event, DbString>();
		//public static readonly Column<Event, DbInt> EventEventID = new Column<Event, DbInt>();
	}
}
