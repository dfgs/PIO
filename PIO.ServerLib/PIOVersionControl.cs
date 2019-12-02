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
					yield return new CreateTable<FactoryTable>(FactoryTable.FactoryID, FactoryTable.PlanetID, FactoryTable.FactoryTypeID, FactoryTable.HealthPoints);
					yield return new CreateTable<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity);
					yield return new CreateTable<MaterialTable>(MaterialTable.MaterialID, MaterialTable.FactoryTypeID, MaterialTable.ResourceTypeID, MaterialTable.Quantity);

					yield return new CreateTable<TaskTypeTable>(TaskTypeTable.TaskTypeID, TaskTypeTable.Name);
					yield return new CreateTable<EventTable>(EventTable.EventID,EventTable.Name);

					yield return new CreateTable<TaskTable>(TaskTable.TaskID, TaskTable.FactoryID, TaskTable.TaskTypeID, TaskTable.ETA,TaskTable.TargetFactoryID,TaskTable.TargetResourceTypeID);

					break;
				case 2:
					yield return new CreateRelation<PlanetTable, FactoryTable, int>(PlanetTable.PlanetID, FactoryTable.PlanetID);
					yield return new CreateRelation<FactoryTable, StackTable, int>(FactoryTable.FactoryID, StackTable.FactoryID);
					yield return new CreateRelation<ResourceTypeTable, StackTable, int>(ResourceTypeTable.ResourceTypeID, StackTable.ResourceTypeID);
					yield return new CreateRelation<FactoryTypeTable, MaterialTable, int>(FactoryTypeTable.FactoryTypeID, MaterialTable.FactoryTypeID);
					yield return new CreateRelation<ResourceTypeTable, MaterialTable, int>(ResourceTypeTable.ResourceTypeID, MaterialTable.ResourceTypeID);
					yield return new CreateRelation<FactoryTypeTable, FactoryTable, int>(FactoryTypeTable.FactoryTypeID, FactoryTable.FactoryTypeID);

					yield return new CreateRelation<FactoryTable, TaskTable, int>(FactoryTable.FactoryID, TaskTable.FactoryID);
					yield return new CreateRelation<TaskTypeTable, TaskTable, int>(TaskTypeTable.TaskTypeID, TaskTable.TaskTypeID);
					yield return new CreateRelation<FactoryTable, TaskTable, int>(FactoryTable.FactoryID, TaskTable.TargetFactoryID);
					yield return new CreateRelation<ResourceTypeTable, TaskTable, int>(ResourceTypeTable.ResourceTypeID, TaskTable.TargetResourceTypeID);


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

					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, 0).Set(TaskTypeTable.Name, "Check materials");
					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, 1).Set(TaskTypeTable.Name, "Search material");
					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, 2).Set(TaskTypeTable.Name, "Collect material");
					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, 3).Set(TaskTypeTable.Name, "Build");

					yield return new Insert<EventTable>().Set(EventTable.EventID, 0).Set(EventTable.Name, "False");
					yield return new Insert<EventTable>().Set(EventTable.EventID, 1).Set(EventTable.Name, "True");


					yield return new Insert<FactoryTable>().Set(FactoryTable.PlanetID, planetID).Set(FactoryTable.FactoryTypeID,0).Set(FactoryTable.HealthPoints, 0);
					yield return new SelectIdentity<FactoryTable>((result) => factoryID= Convert.ToInt32(result));

					yield return new Insert<StackTable>().Set(StackTable.FactoryID, factoryID).Set(StackTable.ResourceTypeID, 0).Set(StackTable.Quantity, 10);
					yield return new Insert<StackTable>().Set(StackTable.FactoryID, factoryID).Set(StackTable.ResourceTypeID, 1).Set(StackTable.Quantity, 5);

					yield return new Insert<TaskTable>().Set(TaskTable.FactoryID, factoryID).Set(TaskTable.TaskTypeID, 0).Set(TaskTable.ETA, DateTime.Now);


					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, 0).Set(MaterialTable.ResourceTypeID, 0).Set(MaterialTable.Quantity, 1);
					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, 0).Set(MaterialTable.ResourceTypeID, 1).Set(MaterialTable.Quantity, 2);

					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, 1).Set(MaterialTable.ResourceTypeID, 0).Set(MaterialTable.Quantity, 1);

					break;
				


			}
		}


	}
}
