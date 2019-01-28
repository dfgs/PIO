using NetORMLib.Databases;
using NetORMLib.Queries;
using NetORMLib.VersionControl;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib
{
	public class PIOVersionControl : VersionControl
	{
		public PIOVersionControl(IDatabase Database) : base(Database)
		{
		}

		public override int GetTargetRevision()
		{
			return 3;
		}

		protected override IEnumerable<IQuery> OnUpgradeTo(int Version)
		{
			int planetID=-1;
			int factoryID=-1;

			switch(Version)
			{
				case 1:
					yield return new CreateTable<Planet>(Planet.PlanetID, Planet.Name);
					yield return new CreateTable<Resource>(Resource.ResourceID, Resource.Name);
					yield return new CreateTable<Factory>(Factory.FactoryID, Factory.PlanetID, Factory.Name);
					yield return new CreateTable<Task>( Task.FactoryID,Task.TaskID, Task.Name);
					yield return new CreateTable<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity);

					yield return new CreateTable<State>(State.StateID, State.Name);
					yield return new CreateTable<Event>(Event.EventID,Event.Name);
					yield return new CreateTable<Transition>(Transition.TransitionID,Transition.StateID,Transition.NextStateID,Transition.EventID);
					break;
				case 2:
					yield return new CreateRelation<Planet, Factory, int>(Planet.PlanetID, Factory.PlanetID);
					yield return new CreateRelation<Factory, Task, int>(Factory.FactoryID, Task.FactoryID);
					yield return new CreateRelation<Factory, Stack, int>(Factory.FactoryID, Stack.FactoryID);
					yield return new CreateRelation<Resource, Stack, int>(Resource.ResourceID, Stack.ResourceID);
					yield return new CreateRelation<State, Transition, int>(State.StateID, Transition.StateID);
					yield return new CreateRelation<State, Transition, int>(State.StateID, Transition.NextStateID);
					yield return new CreateRelation<Event, Transition, int>(Event.EventID, Transition.EventID);

					break;
				case 3:
					yield return new Insert<Planet>().Set(Planet.Name, "Default");
					yield return new SelectIdentity<Planet>((result) => planetID = Convert.ToInt32(result));

					yield return new Insert<Resource>().Set(Resource.ResourceID, 0).Set(Resource.Name, "Wood");
					yield return new Insert<Resource>().Set(Resource.ResourceID, 1).Set(Resource.Name, "Stone");
					yield return new Insert<Resource>().Set(Resource.ResourceID, 2).Set(Resource.Name, "Coal");
					yield return new Insert<Resource>().Set(Resource.ResourceID, 3).Set(Resource.Name, "Plank");

					yield return new Insert<Factory>().Set(Factory.PlanetID, planetID).Set(Factory.Name, "Stockpile");
					yield return new SelectIdentity<Factory>((result) => factoryID= Convert.ToInt32(result));

					yield return new Insert<Stack>().Set(Stack.FactoryID, factoryID).Set(Stack.ResourceID, 0).Set(Stack.Quantity, 10);
					yield return new Insert<Stack>().Set(Stack.FactoryID, factoryID).Set(Stack.ResourceID, 1).Set(Stack.Quantity, 5);

					yield return new Insert<Event>().Set(Event.EventID, 0).Set(Event.Name, "False");
					yield return new Insert<Event>().Set(Event.EventID, 1).Set(Event.Name, "True");

					break;
				


			}
		}


	}
}
