using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Filters;
using NetORMLib.Queries;
using PIO.Models;
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

		public Row<Transition> GetTransition(int TransitionID)
		{
			ISelect<Transition> query;
			LogEnter();

			query = new Select<Transition>(Transition.TransitionID,Transition.StateID, Transition.EventID,Transition.NextStateID).Where(Transition.TransitionID.IsEqualTo(TransitionID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}
		public Row<Transition> GetTransition(int StateID, int EventID)
		{
			ISelect<Transition> query;
			LogEnter();

			query = new Select<Transition>(Transition.TransitionID, Transition.StateID, Transition.EventID, Transition.NextStateID).Where(new AndFilter<Transition>(Transition.StateID.IsEqualTo(StateID), Transition.EventID.IsEqualTo(EventID)));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}




	}
}
