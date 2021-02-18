using NetORMLib;
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
			int buildingID = -1;
			int workerID = -1;

			switch (Version)
			{
				case 1:
					yield return new CreateTable(PIODB.PlanetTable, PlanetTable.PlanetID, PlanetTable.Name,PlanetTable.Width,PlanetTable.Height);
					yield return new CreateTable(PIODB.ResourceTypeTable, ResourceTypeTable.ResourceTypeID, ResourceTypeTable.Name);
					yield return new CreateTable(PIODB.FactoryTypeTable, FactoryTypeTable.FactoryTypeID, FactoryTypeTable.Name, FactoryTypeTable.MaterialSetID, FactoryTypeTable.HealthPoints, FactoryTypeTable.BuildSteps);
					yield return new CreateTable(PIODB.BuildingTypeTable, BuildingTypeTable.BuildingTypeID, BuildingTypeTable.Name, BuildingTypeTable.MaterialSetID, BuildingTypeTable.HealthPoints, BuildingTypeTable.BuildSteps);
					yield return new CreateTable(PIODB.TaskTypeTable, TaskTypeTable.TaskTypeID, TaskTypeTable.Name);

					yield return new CreateTable(PIODB.CellTable, CellTable.CellID, CellTable.PlanetID, CellTable.X, CellTable.Y);
					yield return new CreateTable(PIODB.BuildingTable, BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.BuildingTypeID, BuildingTable.X, BuildingTable.Y, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps);
					yield return new CreateTable(PIODB.FactoryTable, FactoryTable.FactoryID, FactoryTable.BuildingID, FactoryTable.FactoryTypeID);
					yield return new CreateTable(PIODB.WorkerTable, WorkerTable.WorkerID, WorkerTable.PlanetID,WorkerTable.X,WorkerTable.Y, WorkerTable.ResourceTypeID);
					yield return new CreateTable(PIODB.StackTable, StackTable.StackID, StackTable.BuildingID, StackTable.ResourceTypeID, StackTable.Quantity);
					yield return new CreateTable(PIODB.MaterialTable, MaterialTable.MaterialID, MaterialTable.BuildingTypeID, MaterialTable.ResourceTypeID, MaterialTable.Quantity);
					yield return new CreateTable(PIODB.TaskTable, TaskTable.TaskID,TaskTable.TaskTypeID, TaskTable.WorkerID,TaskTable.X,TaskTable.Y,TaskTable.BuildingID, TaskTable.ResourceTypeID,TaskTable.BuildingTypeID,   TaskTable.ETA);

					yield return new CreateTable(PIODB.IngredientTable, IngredientTable.IngredientID, IngredientTable.BuildingTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity);
					yield return new CreateTable(PIODB.ProductTable, ProductTable.ProductID, ProductTable.BuildingTypeID, ProductTable.ResourceTypeID, ProductTable.Quantity, ProductTable.Duration);
					break;
				case 2:
					yield return new CreateRelation<int>(PIODB.WorkerTable, PlanetTable.PlanetID, WorkerTable.PlanetID);
					yield return new CreateRelation<ResourceTypeIDs>(PIODB.WorkerTable, ResourceTypeTable.ResourceTypeID, WorkerTable.ResourceTypeID);
					yield return new CreateRelation<int>(PIODB.StackTable, BuildingTable.BuildingID, StackTable.BuildingID);
					yield return new CreateRelation<ResourceTypeIDs>(PIODB.StackTable, ResourceTypeTable.ResourceTypeID, StackTable.ResourceTypeID);

					//yield return new CreateRelation<int>(PIODB.FactoryTypeTable, MaterialTable.MaterialSetID, FactoryTypeTable.MaterialSetID);
					//yield return new CreateRelation<int>(PIODB.FarmTypeTable, MaterialTable.MaterialSetID, FarmTypeTable.MaterialSetID);

					yield return new CreateRelation<FactoryTypeIDs>(PIODB.FactoryTable, FactoryTypeTable.FactoryTypeID, FactoryTable.FactoryTypeID);
					yield return new CreateRelation<BuildingTypeIDs>(PIODB.BuildingTable, BuildingTypeTable.BuildingTypeID, BuildingTable.BuildingTypeID);
					yield return new CreateRelation<int>(PIODB.FactoryTable, BuildingTable.BuildingID, FactoryTable.BuildingID);
					yield return new CreateRelation<int>(PIODB.CellTable, PlanetTable.PlanetID, CellTable.PlanetID);
					yield return new CreateRelation<int>(PIODB.BuildingTable, PlanetTable.PlanetID, BuildingTable.PlanetID);

					yield return new CreateRelation<TaskTypeIDs>(PIODB.TaskTable, TaskTypeTable.TaskTypeID, TaskTable.TaskTypeID);

					yield return new CreateRelation<int>(PIODB.TaskTable, WorkerTable.WorkerID, TaskTable.WorkerID);
					yield return new CreateRelation<int>(PIODB.TaskTable, BuildingTable.BuildingID, TaskTable.BuildingID);
					//yield return new CreateRelation<int>(PIODB.TaskTable, FactoryTable.BuildingID, TaskTable.TargetBuildingID);

					yield return new CreateRelation<ResourceTypeIDs>(PIODB.TaskTable, ResourceTypeTable.ResourceTypeID, TaskTable.ResourceTypeID);
					yield return new CreateRelation<BuildingTypeIDs>(PIODB.TaskTable, BuildingTypeTable.BuildingTypeID, TaskTable.BuildingTypeID);

					yield return new CreateRelation<BuildingTypeIDs>(PIODB.IngredientTable, BuildingTypeTable.BuildingTypeID, IngredientTable.BuildingTypeID);
					yield return new CreateRelation<ResourceTypeIDs>(PIODB.IngredientTable, ResourceTypeTable.ResourceTypeID, IngredientTable.ResourceTypeID);
					yield return new CreateRelation<BuildingTypeIDs>(PIODB.MaterialTable, BuildingTypeTable.BuildingTypeID, MaterialTable.BuildingTypeID);
					yield return new CreateRelation<ResourceTypeIDs>(PIODB.MaterialTable, ResourceTypeTable.ResourceTypeID, MaterialTable.ResourceTypeID);
					yield return new CreateRelation<BuildingTypeIDs>(PIODB.ProductTable, BuildingTypeTable.BuildingTypeID, ProductTable.BuildingTypeID);
					yield return new CreateRelation<ResourceTypeIDs>(PIODB.ProductTable, ResourceTypeTable.ResourceTypeID, ProductTable.ResourceTypeID);

					yield return new CreateConstraint(PIODB.CellTable, ColumnConstraints.Unique, CellTable.X, CellTable.Y);
					yield return new CreateConstraint(PIODB.BuildingTable, ColumnConstraints.Unique, BuildingTable.X, BuildingTable.Y);
					yield return new CreateConstraint(PIODB.TaskTable, ColumnConstraints.Unique, TaskTable.WorkerID);

					break;
				case 3:
					#region create ResourceType
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Tree).Set(ResourceTypeTable.Name, "Tree");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(ResourceTypeTable.Name, "Wood");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Stone).Set(ResourceTypeTable.Name, "Stone");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Coal).Set(ResourceTypeTable.Name, "Coal");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Plank).Set(ResourceTypeTable.Name, "Plank");
					#endregion

					#region create BuildingType
					yield return new Insert().Into(PIODB.BuildingTypeTable).Set(BuildingTypeTable.BuildingTypeID, BuildingTypeIDs.Forest).Set(BuildingTypeTable.MaterialSetID, 1).Set(BuildingTypeTable.Name, "Forest").Set(BuildingTypeTable.HealthPoints, 10).Set(BuildingTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.BuildingTypeTable).Set(BuildingTypeTable.BuildingTypeID, BuildingTypeIDs.Stockpile).Set(BuildingTypeTable.MaterialSetID, 2).Set(BuildingTypeTable.Name, "Stockpile").Set(BuildingTypeTable.HealthPoints, 10).Set(BuildingTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.BuildingTypeTable).Set(BuildingTypeTable.BuildingTypeID, BuildingTypeIDs.Sawmill).Set(BuildingTypeTable.MaterialSetID, 3).Set(BuildingTypeTable.Name, "Sawmill").Set(BuildingTypeTable.HealthPoints, 10).Set(BuildingTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.BuildingTypeTable).Set(BuildingTypeTable.BuildingTypeID, BuildingTypeIDs.Stone).Set(BuildingTypeTable.MaterialSetID, 3).Set(BuildingTypeTable.Name, "Stone").Set(BuildingTypeTable.HealthPoints, 10).Set(BuildingTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.BuildingTypeTable).Set(BuildingTypeTable.BuildingTypeID, BuildingTypeIDs.Water).Set(BuildingTypeTable.MaterialSetID, 3).Set(BuildingTypeTable.Name, "Water").Set(BuildingTypeTable.HealthPoints, 10).Set(BuildingTypeTable.BuildSteps, 10);
					#endregion

					#region create Material
					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.BuildingTypeID, BuildingTypeIDs.Forest).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);

					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.BuildingTypeID, BuildingTypeIDs.Sawmill).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);
					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.BuildingTypeID, BuildingTypeIDs.Sawmill).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Stone).Set(MaterialTable.Quantity, 2);

					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.BuildingTypeID,  BuildingTypeIDs.Stockpile).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);
					#endregion

					#region create FactoryType
					yield return new Insert().Into(PIODB.FactoryTypeTable).Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(FactoryTypeTable.MaterialSetID,1).Set(FactoryTypeTable.Name, "Forest").Set(FactoryTypeTable.HealthPoints, 10).Set(FactoryTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.FactoryTypeTable).Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Stockpile).Set(FactoryTypeTable.MaterialSetID, 2).Set(FactoryTypeTable.Name, "Stockpile").Set(FactoryTypeTable.HealthPoints, 10).Set(FactoryTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.FactoryTypeTable).Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(FactoryTypeTable.MaterialSetID, 3).Set(FactoryTypeTable.Name, "Sawmill").Set(FactoryTypeTable.HealthPoints, 10).Set(FactoryTypeTable.BuildSteps, 10);
					#endregion




					#region create TaskType
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Idle).Set(TaskTypeTable.Name, "Idle");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Produce).Set(TaskTypeTable.Name, "Produce");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.MoveTo).Set(TaskTypeTable.Name, "MoveTo");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.CreateBuilding).Set(TaskTypeTable.Name, "CreateBuilding");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Build).Set(TaskTypeTable.Name, "Build");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Take).Set(TaskTypeTable.Name, "Take");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Store).Set(TaskTypeTable.Name, "Store");
					#endregion


					#region create Ingredient
					yield return new Insert().Into(PIODB.IngredientTable).Set(IngredientTable.BuildingTypeID, BuildingTypeIDs.Forest).Set(IngredientTable.ResourceTypeID, ResourceTypeIDs.Tree).Set(IngredientTable.Quantity, 1);
					yield return new Insert().Into(PIODB.IngredientTable).Set(IngredientTable.BuildingTypeID, BuildingTypeIDs.Sawmill).Set(IngredientTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(IngredientTable.Quantity, 1);
					#endregion

					#region create Product
					yield return new Insert().Into(PIODB.ProductTable).Set(ProductTable.BuildingTypeID, BuildingTypeIDs.Forest).Set(ProductTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(ProductTable.Quantity, 2).Set(ProductTable.Duration, 30);
					yield return new Insert().Into(PIODB.ProductTable).Set(ProductTable.BuildingTypeID, BuildingTypeIDs.Sawmill).Set(ProductTable.ResourceTypeID, ResourceTypeIDs.Plank).Set(ProductTable.Quantity, 2).Set(ProductTable.Duration, 30);
					#endregion

					#region create startup Planet
					yield return new Insert().Into(PIODB.PlanetTable).Set(PlanetTable.Name, "Default").Set(PlanetTable.Width,50).Set(PlanetTable.Height,50);
					yield return new SelectIdentity((result) => planetID = Convert.ToInt32(result));
					#endregion

					#region create cells
					for(int x=0;x<50;x++)
					{
						for(int y=0;y<50;y++)
						{
							yield return new Insert().Into(PIODB.CellTable).Set(CellTable.PlanetID, planetID).Set(CellTable.X, x).Set(CellTable.Y, y);
						}
					}

					#endregion

	
					#region create startup Farm
					yield return new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.BuildingTypeID, BuildingTypeIDs.Forest).Set(BuildingTable.X, 4).Set(BuildingTable.Y, 4).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
					yield return new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
					#endregion


					#region create startup Factories
					yield return new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.BuildingTypeID,BuildingTypeIDs.Forest).Set(BuildingTable.X, 0).Set(BuildingTable.Y, 0).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
					yield return new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
					yield return new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, buildingID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Forest);
					yield return new SelectIdentity((result) => factoryID = Convert.ToInt32(result));
					#region fill startup factories with material
					yield return new Insert().Into(PIODB.StackTable).Set(StackTable.BuildingID, buildingID).Set(StackTable.ResourceTypeID, ResourceTypeIDs.Tree).Set(StackTable.Quantity, 15);
					#endregion

					yield return new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.BuildingTypeID, BuildingTypeIDs.Stockpile).Set(BuildingTable.X, 1).Set(BuildingTable.Y, 0).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
					yield return new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
					yield return new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, buildingID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Stockpile);

					yield return new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.BuildingTypeID, BuildingTypeIDs.Sawmill).Set(BuildingTable.X, 2).Set(BuildingTable.Y, 0).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
					yield return new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
					yield return new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, buildingID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Sawmill);

					#endregion

					#region create startup Worker
					yield return new Insert().Into(PIODB.WorkerTable).Set(WorkerTable.PlanetID,planetID).Set(WorkerTable.X,0).Set(WorkerTable.Y,1);
					yield return new SelectIdentity((result) => workerID = Convert.ToInt32(result));
					yield return new Insert().Into(PIODB.WorkerTable).Set(WorkerTable.PlanetID, planetID).Set(WorkerTable.X, 0).Set(WorkerTable.Y, 2);
					#endregion





					//yield return new Insert().Into(TaskTable)Set(TaskTable.WorkerID, factoryID).Set(TaskTable.ETA, DateTime.Now);


				break;
				


			}
		}


	}
}
