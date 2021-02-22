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
			return 2;
		}

		protected override IEnumerable<IQuery> OnUpgradeTo(int Version)
		{

			switch (Version)
			{
				case 1:
					yield return new CreateTable(PIODB.PlanetTable, PlanetTable.PlanetID, PlanetTable.Name,PlanetTable.Width,PlanetTable.Height);
					yield return new CreateTable(PIODB.ResourceTypeTable, ResourceTypeTable.ResourceTypeID, ResourceTypeTable.Name);
					yield return new CreateTable(PIODB.BuildingTypeTable, BuildingTypeTable.BuildingTypeID, BuildingTypeTable.Name, BuildingTypeTable.HealthPoints, BuildingTypeTable.BuildSteps,BuildingTypeTable.IsFactory,BuildingTypeTable.IsFarm );
					yield return new CreateTable(PIODB.TaskTypeTable, TaskTypeTable.TaskTypeID, TaskTypeTable.Name);

					yield return new CreateTable(PIODB.CellTable, CellTable.CellID, CellTable.PlanetID, CellTable.X, CellTable.Y);
					yield return new CreateTable(PIODB.BuildingTable, BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.BuildingTypeID, BuildingTable.X, BuildingTable.Y, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps);
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

					yield return new CreateRelation<BuildingTypeIDs>(PIODB.BuildingTable, BuildingTypeTable.BuildingTypeID, BuildingTable.BuildingTypeID);
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

				


			}
		}


	}
}
