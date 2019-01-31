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
	public class StateModule : DatabaseModule,IStateModule
	{

		public StateModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row GetState(int StateID)
		{
			ISelect query;
			LogEnter();

			query = new Select<State>(State.StateID, State.Name).Where(State.StateID.IsEqualTo(StateID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

	



	}
}
