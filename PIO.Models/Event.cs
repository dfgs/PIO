using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.Models
{
	public class Event
	{
		public static readonly Column<Event, DbInt> EventID = new Column<Event, DbInt>() { IsPrimaryKey = true, IsIdentity = false };
		public static readonly Column<Event, DbString> Name = new Column<Event, DbString>() {DefaultValue="New Event" };
		//public static readonly Column<Event, int> EventEventID = new Column<Event, int>();
	}
}
