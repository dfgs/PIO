using NetORMLib.Databases;
using NetORMLib.DbTypes;
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
					yield return new CreateTable<FactoryType>(FactoryType.FactoryTypeID, FactoryType.Name);
					yield return new CreateTable<Material>(Material.MaterialID, Material.FactoryTypeID, Material.ResourceID, Material.Quantity);
					yield return new CreateTable<Factory>(Factory.FactoryID, Factory.FactoryTypeID, Factory.PlanetID, Factory.Name,Factory.StateID);
					yield return new CreateTable<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity);

					yield return new CreateTable<Task>(Task.TaskID, Task.Name);
					yield return new CreateTable<State>(State.StateID, State.Name,State.TaskID,State.Duration);
					yield return new CreateTable<Event>(Event.EventID,Event.Name);
					yield return new CreateTable<Transition>(Transition.TransitionID,Transition.StateID,Transition.NextStateID,Transition.EventID);

					yield return new CreateTable<ScheduledTask>(ScheduledTask.ScheduledTaskID, ScheduledTask.FactoryID, ScheduledTask.TaskID, ScheduledTask.ETA);

					break;
				case 2:
					yield return new CreateRelation<Planet, Factory, DbInt>(Planet.PlanetID, Factory.PlanetID);
					yield return new CreateRelation<FactoryType, Factory, DbInt>(FactoryType.FactoryTypeID, Factory.FactoryTypeID);
					yield return new CreateRelation<FactoryType, Material, DbInt>(FactoryType.FactoryTypeID, Material.FactoryTypeID);
					yield return new CreateRelation<Factory, Stack, DbInt>(Factory.FactoryID, Stack.FactoryID);
					yield return new CreateRelation<Resource, Stack, DbInt>(Resource.ResourceID, Stack.ResourceID);
					yield return new CreateRelation<Resource, Material, DbInt>(Resource.ResourceID, Material.ResourceID);
					yield return new CreateRelation<State, Transition, DbInt>(State.StateID, Transition.StateID);
					yield return new CreateRelation<State, Transition, DbInt>(State.StateID, Transition.NextStateID);
					yield return new CreateRelation<Event, Transition, DbInt>(Event.EventID, Transition.EventID);
					yield return new CreateRelation<State, Factory, DbInt>(State.StateID, Factory.StateID);

					yield return new CreateRelation<Factory, ScheduledTask, DbInt>(Factory.FactoryID, ScheduledTask.FactoryID);
					yield return new CreateRelation<Task, ScheduledTask, DbInt>(Task.TaskID, ScheduledTask.TaskID);

					yield return new CreateRelation<Task, State, DbInt>(Task.TaskID, State.TaskID);

					break;
				case 3:
					yield return new Insert<Planet>().Set(Planet.Name, "Default");
					yield return new SelectIdentity<Planet>((result) => planetID = Convert.ToInt32(result));

					yield return new Insert<Resource>().Set(Resource.ResourceID, 0).Set(Resource.Name, "Wood");
					yield return new Insert<Resource>().Set(Resource.ResourceID, 1).Set(Resource.Name, "Stone");
					yield return new Insert<Resource>().Set(Resource.ResourceID, 2).Set(Resource.Name, "Coal");
					yield return new Insert<Resource>().Set(Resource.ResourceID, 3).Set(Resource.Name, "Plank");

					yield return new Insert<Task>().Set(Task.TaskID, 0).Set(Task.Name, "NOP");

					yield return new Insert<State>().Set(State.StateID, 0).Set(State.TaskID, 0).Set(State.Duration,5).Set(State.Name, "Checking material");
					yield return new Insert<State>().Set(State.StateID, 1).Set(State.TaskID, 0).Set(State.Duration, 5).Set(State.Name, "Searching material");
					yield return new Insert<State>().Set(State.StateID, 2).Set(State.TaskID, 0).Set(State.Duration, 5).Set(State.Name, "Suspended (no material)");
					yield return new Insert<State>().Set(State.StateID, 3).Set(State.TaskID, 0).Set(State.Duration, 5).Set(State.Name, "Collecting material");
					yield return new Insert<State>().Set(State.StateID, 4).Set(State.TaskID, 0).Set(State.Duration, 5).Set(State.Name, "Building");
					yield return new Insert<State>().Set(State.StateID, 5).Set(State.TaskID, 0).Set(State.Duration, 5).Set(State.Name, "Producing");

					yield return new Insert<Event>().Set(Event.EventID, 0).Set(Event.Name, "False");
					yield return new Insert<Event>().Set(Event.EventID, 1).Set(Event.Name, "True");

					yield return new Insert<Transition>().Set(Transition.StateID, 0).Set(Transition.NextStateID, 1).Set(Transition.EventID, 0);
					yield return new Insert<Transition>().Set(Transition.StateID, 0).Set(Transition.NextStateID, 4).Set(Transition.EventID, 1);
					yield return new Insert<Transition>().Set(Transition.StateID, 1).Set(Transition.NextStateID, 2).Set(Transition.EventID, 0);
					yield return new Insert<Transition>().Set(Transition.StateID, 1).Set(Transition.NextStateID, 3).Set(Transition.EventID, 1);
					yield return new Insert<Transition>().Set(Transition.StateID, 2).Set(Transition.NextStateID, 1).Set(Transition.EventID, 1);
					yield return new Insert<Transition>().Set(Transition.StateID, 3).Set(Transition.NextStateID, 0).Set(Transition.EventID, 1);
					yield return new Insert<Transition>().Set(Transition.StateID, 4).Set(Transition.NextStateID, 0).Set(Transition.EventID, 0);
					yield return new Insert<Transition>().Set(Transition.StateID, 4).Set(Transition.NextStateID, 5).Set(Transition.EventID, 1);

					yield return new Insert<FactoryType>().Set(FactoryType.FactoryTypeID, 0).Set(FactoryType.Name, "Stockpile");
					yield return new Insert<FactoryType>().Set(FactoryType.FactoryTypeID, 1).Set(FactoryType.Name, "Wood cutter");
					yield return new Insert<FactoryType>().Set(FactoryType.FactoryTypeID, 2).Set(FactoryType.Name, "Stone cutter");


					yield return new Insert<Material>().Set(Material.FactoryTypeID, 1).Set(Material.ResourceID,0).Set(Material.Quantity,5);

					yield return new Insert<Factory>().Set(Factory.PlanetID, planetID).Set(Factory.Name, "Stockpile").Set(Factory.FactoryTypeID,0).Set(Factory.StateID,5);
					yield return new SelectIdentity<Factory>((result) => factoryID= Convert.ToInt32(result));

					yield return new Insert<Stack>().Set(Stack.FactoryID, factoryID).Set(Stack.ResourceID, 0).Set(Stack.Quantity, 10);
					yield return new Insert<Stack>().Set(Stack.FactoryID, factoryID).Set(Stack.ResourceID, 1).Set(Stack.Quantity, 5);

					break;
				


			}
		}


	}
}
