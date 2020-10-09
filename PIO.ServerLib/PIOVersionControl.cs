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
			int factoryID = -1;
			int workerID = -1;

			switch (Version)
			{
				case 1:
					yield return new CreateTable<PlanetTable>(PlanetTable.PlanetID, PlanetTable.Name);
					yield return new CreateTable<ResourceTypeTable>(ResourceTypeTable.ResourceTypeID, ResourceTypeTable.Name);
					yield return new CreateTable<FactoryTypeTable>(FactoryTypeTable.FactoryTypeID, FactoryTypeTable.Name, FactoryTypeTable.HealthPoints);
					yield return new CreateTable<TaskTypeTable>(TaskTypeTable.TaskTypeID, TaskTypeTable.Name);
					yield return new CreateTable<FactoryTable>(FactoryTable.FactoryID, FactoryTable.PlanetID, FactoryTable.FactoryTypeID, FactoryTable.HealthPoints);
					yield return new CreateTable<WorkerTable>(WorkerTable.WorkerID, WorkerTable.FactoryID);
					yield return new CreateTable<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity);
					yield return new CreateTable<MaterialTable>(MaterialTable.MaterialID, MaterialTable.FactoryTypeID, MaterialTable.ResourceTypeID, MaterialTable.Quantity);
					yield return new CreateTable<TaskTable>(TaskTable.TaskID,TaskTable.TaskTypeID, TaskTable.WorkerID, TaskTable.TargetFactoryID,TaskTable.ResourceTypeID,  TaskTable.ETA);

					yield return new CreateTable<IngredientTable>(IngredientTable.IngredientID, IngredientTable.FactoryTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity);
					yield return new CreateTable<ProductTable>(ProductTable.ProductID, ProductTable.FactoryTypeID, ProductTable.ResourceTypeID, ProductTable.Quantity, ProductTable.Duration);
					break;
				case 2:
					yield return new CreateRelation<PlanetTable, FactoryTable, int>(PlanetTable.PlanetID, FactoryTable.PlanetID);
					yield return new CreateRelation<FactoryTable, WorkerTable, int>(FactoryTable.FactoryID, WorkerTable.FactoryID);
					yield return new CreateRelation<FactoryTable, StackTable, int>(FactoryTable.FactoryID, StackTable.FactoryID);
					yield return new CreateRelation<ResourceTypeTable, StackTable, ResourceTypeIDs>(ResourceTypeTable.ResourceTypeID, StackTable.ResourceTypeID);
					yield return new CreateRelation<FactoryTypeTable, MaterialTable, FactoryTypeIDs>(FactoryTypeTable.FactoryTypeID, MaterialTable.FactoryTypeID);
					yield return new CreateRelation<ResourceTypeTable, MaterialTable, ResourceTypeIDs>(ResourceTypeTable.ResourceTypeID, MaterialTable.ResourceTypeID);
					yield return new CreateRelation<FactoryTypeTable, FactoryTable, FactoryTypeIDs>(FactoryTypeTable.FactoryTypeID, FactoryTable.FactoryTypeID);
					yield return new CreateRelation<TaskTypeTable, TaskTable, TaskTypeIDs>(TaskTypeTable.TaskTypeID, TaskTable.TaskTypeID);

					yield return new CreateRelation<WorkerTable, TaskTable, int>(WorkerTable.WorkerID, TaskTable.WorkerID);
					yield return new CreateRelation<FactoryTable, TaskTable, int>(FactoryTable.FactoryID, TaskTable.TargetFactoryID);
					yield return new CreateRelation<ResourceTypeTable, TaskTable, ResourceTypeIDs>(ResourceTypeTable.ResourceTypeID, TaskTable.ResourceTypeID);

					yield return new CreateRelation<FactoryTypeTable, IngredientTable, FactoryTypeIDs>(FactoryTypeTable.FactoryTypeID, IngredientTable.FactoryTypeID);
					yield return new CreateRelation<ResourceTypeTable, IngredientTable, ResourceTypeIDs>(ResourceTypeTable.ResourceTypeID, IngredientTable.ResourceTypeID);
					yield return new CreateRelation<FactoryTypeTable, ProductTable, FactoryTypeIDs>(FactoryTypeTable.FactoryTypeID, ProductTable.FactoryTypeID);
					yield return new CreateRelation<ResourceTypeTable, ProductTable, ResourceTypeIDs>(ResourceTypeTable.ResourceTypeID, ProductTable.ResourceTypeID);


					break;
				case 3:
					#region create ResourceType
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Tree).Set(ResourceTypeTable.Name, "Tree");
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(ResourceTypeTable.Name, "Wood");
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Stone).Set(ResourceTypeTable.Name, "Stone");
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Coal).Set(ResourceTypeTable.Name, "Coal");
					yield return new Insert<ResourceTypeTable>().Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Plank).Set(ResourceTypeTable.Name, "Plank");
					#endregion

					#region create FactoryType
					yield return new Insert<FactoryTypeTable>().Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(FactoryTypeTable.Name, "Forest").Set(FactoryTypeTable.HealthPoints, 999);
					yield return new Insert<FactoryTypeTable>().Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Stockpile).Set(FactoryTypeTable.Name, "Stockpile").Set(FactoryTypeTable.HealthPoints, 50);
					yield return new Insert<FactoryTypeTable>().Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(FactoryTypeTable.Name, "Sawmill").Set(FactoryTypeTable.HealthPoints, 5);
					#endregion

					#region create TaskType
					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Produce).Set(TaskTypeTable.Name, "Produce");
					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.MoveTo).Set(TaskTypeTable.Name, "MoveTo");
					yield return new Insert<TaskTypeTable>().Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.CarryTo).Set(TaskTypeTable.Name, "CarryTo");
					#endregion

					#region create Ingredient
					yield return new Insert<IngredientTable>().Set(IngredientTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(IngredientTable.ResourceTypeID, ResourceTypeIDs.Tree).Set(IngredientTable.Quantity, 1);
					yield return new Insert<IngredientTable>().Set(IngredientTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(IngredientTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(IngredientTable.Quantity, 1);
					#endregion

					#region create Product
					yield return new Insert<ProductTable>().Set(ProductTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(ProductTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(ProductTable.Quantity, 2).Set(ProductTable.Duration, 60);
					yield return new Insert<ProductTable>().Set(ProductTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(ProductTable.ResourceTypeID, ResourceTypeIDs.Plank).Set(ProductTable.Quantity, 2).Set(ProductTable.Duration, 60);
					#endregion

					#region create startup Planet
					yield return new Insert<PlanetTable>().Set(PlanetTable.Name, "Default");
					yield return new SelectIdentity<PlanetTable>((result) => planetID = Convert.ToInt32(result));
					#endregion

					#region create startup Factories
					yield return new Insert<FactoryTable>().Set(FactoryTable.PlanetID, planetID).Set(FactoryTable.FactoryTypeID, (int)FactoryTypeIDs.Forest).Set(FactoryTable.HealthPoints, 999);
					yield return new SelectIdentity<FactoryTable>((result) => factoryID = Convert.ToInt32(result));
					#region fill startup factories with material
					yield return new Insert<StackTable>().Set(StackTable.FactoryID, factoryID).Set(StackTable.ResourceTypeID, (int)ResourceTypeIDs.Tree).Set(StackTable.Quantity, 100);
					#endregion

					yield return new Insert<FactoryTable>().Set(FactoryTable.PlanetID, planetID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Stockpile).Set(FactoryTable.HealthPoints, 999);
					yield return new Insert<FactoryTable>().Set(FactoryTable.PlanetID, planetID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(FactoryTable.HealthPoints, 999);

					#endregion

					#region create startup Worker
					yield return new Insert<WorkerTable>().Set(WorkerTable.FactoryID,factoryID);
					yield return new SelectIdentity<WorkerTable>((result) => workerID = Convert.ToInt32(result));
					#endregion





					//yield return new Insert<TaskTable>().Set(TaskTable.WorkerID, factoryID).Set(TaskTable.ETA, DateTime.Now);


					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);
					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Stone).Set(MaterialTable.Quantity, 2);

					yield return new Insert<MaterialTable>().Set(MaterialTable.FactoryTypeID, FactoryTypeIDs.Stockpile).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);

					break;
				


			}
		}


	}
}
