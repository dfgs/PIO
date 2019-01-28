using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Event
	{
		public static readonly Column<Event, int> EventID = new Column<Event, int>() { IsPrimaryKey = true, IsIdentity = false };
		public static readonly Column<Event, string> Name = new Column<Event, string>() {DefaultValue="New Event" };
		//public static readonly Column<Event, int> EventEventID = new Column<Event, int>();
	}
}
