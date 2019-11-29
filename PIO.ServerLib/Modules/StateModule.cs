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
	public class StateModule : DatabaseModule,IStateModule
	{

		public StateModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public State GetState(int StateID)
		{
			ISelect<StateTable> query;
			LogEnter();

			query = new Select<StateTable>(StateTable.StateID, StateTable.Name,StateTable.TaskID,StateTable.Duration).Where(StateTable.StateID.IsEqualTo(StateID));
			return TrySelectMany<StateTable,State>(query).OrThrow("Failed to query").FirstOrDefault();
		}

	



	}
}
