using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Filters;
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
	public class TransitionModule : DatabaseModule, ITransitionModule
	{

		public TransitionModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Transition GetTransition(int TransitionID)
		{
			ISelect<TransitionTable> query;
			LogEnter();

			query = new Select<TransitionTable>(TransitionTable.TransitionID,TransitionTable.StateID, TransitionTable.EventID,TransitionTable.NextStateID).Where(TransitionTable.TransitionID.IsEqualTo(TransitionID));
			return Try<TransitionTable,Transition>(query).OrThrow("Failed to query").FirstOrDefault();
		}
		public Transition GetTransition(int StateID, int EventID)
		{
			ISelect<TransitionTable> query;
			LogEnter();

			query = new Select<TransitionTable>(TransitionTable.TransitionID, TransitionTable.StateID, TransitionTable.EventID, TransitionTable.NextStateID).Where(new AndFilter<TransitionTable>(TransitionTable.StateID.IsEqualTo(StateID), TransitionTable.EventID.IsEqualTo(EventID)));
			return Try<TransitionTable, Transition>(query).OrThrow("Failed to query").FirstOrDefault();
		}




	}
}
