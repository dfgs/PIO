using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
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

		public Event GetEvent(int EventID)
		{
			ISelect<EventTable> query;
			LogEnter();

			query = new Select<EventTable>(EventTable.EventID, EventTable.Name).Where(EventTable.EventID.IsEqualTo(EventID));
			return TrySelectMany<EventTable,Event>(query).OrThrow("Failed to query").FirstOrDefault();
		}

	



	}
}
