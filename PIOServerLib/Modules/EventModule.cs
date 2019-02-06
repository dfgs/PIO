using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public class EventModule : DatabaseModule,IEventModule
	{

		public EventModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row GetEvent(int EventID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Event>(Event.EventID, Event.Name).Where(Event.EventID.IsEqualTo(EventID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

	



	}
}
