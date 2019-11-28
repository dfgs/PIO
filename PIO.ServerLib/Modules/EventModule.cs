using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public class EventModule : DatabaseModule,IEventModule
	{

		public EventModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row<Event> GetEvent(int EventID)
		{
			ISelect<Event> query;
			LogEnter();

			query = new Select<Event>(Event.EventID, Event.Name).Where(Event.EventID.IsEqualTo(EventID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

	



	}
}
