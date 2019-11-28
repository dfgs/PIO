using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class EventTable
	{
		public static readonly Column<EventTable, int> EventID = new Column<EventTable, int>() { IsPrimaryKey = true, IsIdentity = false };
		public static readonly Column<EventTable, string> Name = new Column<EventTable, string>() {DefaultValue="New Event" };
		//public static readonly Column<Event, int> EventEventID = new Column<Event, int>();
	}
}
