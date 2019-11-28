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
	public class StateModule : DatabaseModule,IStateModule
	{

		public StateModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row<State> GetState(int StateID)
		{
			ISelect<State> query;
			LogEnter();

			query = new Select<State>(State.StateID, State.Name,State.TaskID,State.Duration).Where(State.StateID.IsEqualTo(StateID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

	



	}
}
