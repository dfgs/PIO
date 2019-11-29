using NetORMLib.Databases;

using NetORMLib.Queries;
using NetORMLib.VersionControl;
using PIO.Models;
using PIO.ServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib
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
					yield return new CreateTable<PlanetTable>(PlanetTable.PlanetID, PlanetTable.Name);
					yield return new CreateTable<ResourceTypeTable>(ResourceTypeTable.ResourceTypeID, ResourceTypeTable.Name);
					yield return new CreateTable<FactoryTypeTable>(FactoryTypeTable.FactoryTypeID, FactoryTypeTable.Name, FactoryTypeTable.HealthPoints);
					yield return new CreateTable<FactoryTable>(FactoryTable.FactoryID, FactoryTable.PlanetID, FactoryTable.FactoryTypeID, FactoryTable.HealthPoints, FactoryTable.StateID);
					yield return new CreateTable<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceID, StackTable.Quantity);
					yield return new CreateTable<MaterialTable>(MaterialTable.MaterialID, MaterialTable.FactoryTypeID, MaterialTable.ResourceID, MaterialTable.Quantity);

					yield return new CreateTable<TaskTypeTable>(TaskTypeTable.TaskTypeID, TaskTypeTable.Name);
					yield return new CreateTable<StateTable>(StateTable.StateID, StateTable.Name,StateTable.TaskID,StateTable.Duration);
					yield return new CreateTable<EventTable>(EventTable.EventID,EventTable.Name);
					yield return new CreateTable<TransitionTable>(TransitionTable.TransitionID,TransitionTable.StateID,TransitionTable.NextStateID,TransitionTable.EventID);

					yield return new CreateTable<ScheduledTaskTable>(ScheduledTaskTable.ScheduledTaskID, ScheduledTaskTable.FactoryID, ScheduledTaskTable.TaskID, ScheduledTaskTable.ETA);

					break;
				case 2:
					yield return new CreateRelation<PlanetTable, FactoryTable, int>(PlanetTable.PlanetID, FactoryTable.PlanetID);
					yield return new CreateRelation<FactoryTable, StackTable, int>(FactoryTable.FactoryID, StackTable.FactoryID);
					yield return new CreateRelation<ResourceTypeTable, StackTable, int>(ResourceTypeTable.ResourceTypeID, StackTable.ResourceID);
					yield return new CreateRelation<FactoryTypeTable, MaterialTable, int>(FactoryTypeTable.FactoryTypeID, MaterialTable.FactoryTypeID);
					yield return new CreateRelation<ResourceTypeTable, MaterialTable, int>(ResourceTypeTable.ResourceTypeID, MaterialTable.ResourceID);
					yield return new CreateRelation<StateTable, TransitionTable, int>(StateTable.StateID, TransitionTable.StateID);
					yield return new CreateRelation<StateTable, TransitionTable, int>(StateTable.StateID, TransitionTable.NextStateID);
					yield return new CreateRelation<EventTable, TransitionTable, int>(EventTable.EventID, TransitionTable.EventID);
					yield return new CreateRelation<FactoryTypeTable, FactoryTable, int>(FactoryTypeTable.FactoryTypeID, FactoryTable.FactoryTypeID);
					yield return new CreateRelation<StateTable, FactoryTable, int>(StateTable.StateID, FactoryTable.StateID);

					yield return new CreateRelation<FactoryTable, ScheduledTaskTable, int>(FactoryTable.FactoryID, ScheduledTaskTable.FactoryID);
					yield return new CreateRelation<TaskTypeTable, ScheduledTaskTable, int>(TaskTypeTable.TaskTypeID, ScheduledTaskTable.TaskID);

					yield return new CreateRelation<TaskTypeTable, StateTable, int>(TaskTypeTable.TaskTypeID, StateTable.TaskID);

					break;
				case 3:
					yield return new Insert<PlanetTable>().Set(PlanetTable.Name, "Default");
					yield return new SelectIdentity<PlanetTable>((result) => planetID = Convert.ToInt32(result));

					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, 0).Set(ResourceTypeTable.Name, "Wood");
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, 1).Set(ResourceTypeTable.Name, "Stone");
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, 2).Set(ResourceTypeTable.Name, "Coal");
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, 3).Set(ResourceTypeTable.Name, "Plank");

					yield return new Insert<FactoryTypeTable>().Set(FactoryTypeTable.FactoryTypeID, 0).Set(FactoryTypeTable.Name, "Stockpile").Set(FactoryTypeTable.HealthPoints,50);
					yield return new Insert<FactoryTypeTable>().Set(FactoryTypeTable.FactoryTypeID, 1).Set(FactoryTypeTable.Name, "Wood cuter").Set(FactoryTypeTable.HealthPoints, 5);

					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, 0).Set(TaskTypeTable.Name, "NOP");

					yield return new Insert<StateTable>().Set(StateTable.StateID, 0).Set(StateTable.TaskID, 0).Set(StateTable.Duration,5).Set(StateTable.Name, "Checking material");
					yield return new Insert<StateTable>().Set(StateTable.StateID, 1).Set(StateTable.TaskID, 0).Set(StateTable.Duration, 5).Set(StateTable.Name, "Searching material");
					yield return new Insert<StateTable>().Set(StateTable.StateID, 2).Set(StateTable.TaskID, 0).Set(StateTable.Duration, 5).Set(StateTable.Name, "Suspended (no material)");
					yield return new Insert<StateTable>().Set(StateTable.StateID, 3).Set(StateTable.TaskID, 0).Set(StateTable.Duration, 5).Set(StateTable.Name, "Collecting material");
					yield return new Insert<StateTable>().Set(StateTable.StateID, 4).Set(StateTable.TaskID, 0).Set(StateTable.Duration, 5).Set(StateTable.Name, "Building");
					yield return new Insert<StateTable>().Set(StateTable.StateID, 5).Set(StateTable.TaskID, 0).Set(StateTable.Duration, 5).Set(StateTable.Name, "Producing");

					yield return new Insert<EventTable>().Set(EventTable.EventID, 0).Set(EventTable.Name, "False");
					yield return new Insert<EventTable>().Set(EventTable.EventID, 1).Set(EventTable.Name, "True");

					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 0).Set(TransitionTable.NextStateID, 1).Set(TransitionTable.EventID, 0);
					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 0).Set(TransitionTable.NextStateID, 4).Set(TransitionTable.EventID, 1);
					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 1).Set(TransitionTable.NextStateID, 2).Set(TransitionTable.EventID, 0);
					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 1).Set(TransitionTable.NextStateID, 3).Set(TransitionTable.EventID, 1);
					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 2).Set(TransitionTable.NextStateID, 1).Set(TransitionTable.EventID, 1);
					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 3).Set(TransitionTable.NextStateID, 0).Set(TransitionTable.EventID, 1);
					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 4).Set(TransitionTable.NextStateID, 0).Set(TransitionTable.EventID, 0);
					yield return new Insert<TransitionTable>().Set(TransitionTable.StateID, 4).Set(TransitionTable.NextStateID, 5).Set(TransitionTable.EventID, 1);


					yield return new Insert<FactoryTable>().Set(FactoryTable.PlanetID, planetID).Set(FactoryTable.FactoryTypeID,0).Set(FactoryTable.HealthPoints, 0).Set(FactoryTable.StateID, 5);
					yield return new SelectIdentity<FactoryTable>((result) => factoryID= Convert.ToInt32(result));

					yield return new Insert<StackTable>().Set(StackTable.FactoryID, factoryID).Set(StackTable.ResourceID, 0).Set(StackTable.Quantity, 10);
					yield return new Insert<StackTable>().Set(StackTable.FactoryID, factoryID).Set(StackTable.ResourceID, 1).Set(StackTable.Quantity, 5);

					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, 0).Set(MaterialTable.ResourceID, 0).Set(MaterialTable.Quantity, 1);
					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, 0).Set(MaterialTable.ResourceID, 1).Set(MaterialTable.Quantity, 2);

					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, 1).Set(MaterialTable.ResourceID, 0).Set(MaterialTable.Quantity, 1);

					break;
				


			}
		}


	}
}
