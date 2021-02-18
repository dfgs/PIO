using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using NetORMLib.VersionControl;
using PIO.Bots.ServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.ServerLib
{
	public class BotsVersionControl : VersionControl
	{
		public BotsVersionControl(IDatabase Database) : base(Database)
		{
		}

		public override int GetTargetRevision()
		{
			return 2;
		}

		protected override IEnumerable<IQuery> OnUpgradeTo(int Version)
		{
			/*int planetID = -1;
			int factoryID = -1;
			int buildingID = -1;
			int workerID = -1;*/

			switch (Version)
			{
				case 1:
					yield return new CreateTable(BotsDB.BotTable, BotTable.BotID, BotTable.WorkerID);
					yield return new CreateTable(BotsDB.OrderTable, OrderTable.OrderID, OrderTable.BotID);
					yield return new CreateTable(BotsDB.ProduceOrderTable, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.PlanetID, ProduceOrderTable.BuildingID);
					yield return new CreateTable(BotsDB.BuildOrderTable, BuildOrderTable.BuildOrderID, BuildOrderTable.OrderID, BuildOrderTable.BuildingTypeID, BuildOrderTable.PlanetID, BuildOrderTable.X, BuildOrderTable.Y);
					break;
				case 2:
					yield return new CreateRelation<int>(BotsDB.OrderTable, OrderTable.OrderID, ProduceOrderTable.OrderID);
					yield return new CreateRelation<int>(BotsDB.OrderTable, OrderTable.OrderID, BuildOrderTable.OrderID);

					yield return new CreateRelation<int>(BotsDB.BotTable, BotTable.BotID, OrderTable.BotID);


					yield return new CreateConstraint(BotsDB.ProduceOrderTable, ColumnConstraints.Unique, ProduceOrderTable.PlanetID, ProduceOrderTable.BuildingID);
					yield return new CreateConstraint(BotsDB.BuildOrderTable, ColumnConstraints.Unique, BuildOrderTable.PlanetID, BuildOrderTable.X, BuildOrderTable.Y);

					break;
				/*case 3:
					#region create ResourceType
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Tree).Set(ResourceTypeTable.Name, "Tree");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(ResourceTypeTable.Name, "Wood");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Stone).Set(ResourceTypeTable.Name, "Stone");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Coal).Set(ResourceTypeTable.Name, "Coal");
					yield return new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, ResourceTypeIDs.Plank).Set(ResourceTypeTable.Name, "Plank");
					#endregion

					#region create BuildingType
					yield return new Insert().Into(PIODB.BuildingTypeTable).Set(BuildingTypeTable.BuildingTypeID, BuildingTypeIDs.Factory).Set(BuildingTypeTable.Name, "Forest");
					#endregion

					#region create FactoryType
					yield return new Insert().Into(PIODB.FactoryTypeTable).Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(FactoryTypeTable.Name, "Forest").Set(FactoryTypeTable.HealthPoints, 10).Set(FactoryTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.FactoryTypeTable).Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Stockpile).Set(FactoryTypeTable.Name, "Stockpile").Set(FactoryTypeTable.HealthPoints, 10).Set(FactoryTypeTable.BuildSteps, 10);
					yield return new Insert().Into(PIODB.FactoryTypeTable).Set(FactoryTypeTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(FactoryTypeTable.Name, "Sawmill").Set(FactoryTypeTable.HealthPoints, 10).Set(FactoryTypeTable.BuildSteps, 10);
					#endregion

					#region create TaskType
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Idle).Set(TaskTypeTable.Name, "Idle");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Produce).Set(TaskTypeTable.Name, "Produce");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.MoveTo).Set(TaskTypeTable.Name, "MoveTo");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.CarryTo).Set(TaskTypeTable.Name, "CarryTo");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.CreateBuilding).Set(TaskTypeTable.Name, "CreateBuilding");
					yield return new Insert().Into(PIODB.TaskTypeTable).Set(TaskTypeTable.TaskTypeID, TaskTypeIDs.Build).Set(TaskTypeTable.Name, "Build");
					#endregion

					#region create Material
					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);
					#endregion

					#region create Ingredient
					yield return new Insert().Into(PIODB.IngredientTable).Set(IngredientTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(IngredientTable.ResourceTypeID, ResourceTypeIDs.Tree).Set(IngredientTable.Quantity, 1);
					yield return new Insert().Into(PIODB.IngredientTable).Set(IngredientTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(IngredientTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(IngredientTable.Quantity, 1);
					#endregion

					#region create Product
					yield return new Insert().Into(PIODB.ProductTable).Set(ProductTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(ProductTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(ProductTable.Quantity, 2).Set(ProductTable.Duration, 6);
					yield return new Insert().Into(PIODB.ProductTable).Set(ProductTable.FactoryTypeID, FactoryTypeIDs.Sawmill).Set(ProductTable.ResourceTypeID, ResourceTypeIDs.Plank).Set(ProductTable.Quantity, 2).Set(ProductTable.Duration, 6);
					#endregion

					#region create startup Planet
					yield return new Insert().Into(PIODB.PlanetTable).Set(PlanetTable.Name, "Default");
					yield return new SelectIdentity((result) => planetID = Convert.ToInt32(result));
					#endregion

					#region create startup Factories
					yield return new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.X, 0).Set(BuildingTable.Y, 0).Set(BuildingTable.BuildingTypeID, BuildingTypeIDs.Factory).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
					yield return new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
					yield return new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, buildingID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Forest);
					yield return new SelectIdentity((result) => factoryID = Convert.ToInt32(result));
					#region fill startup factories with material
					yield return new Insert().Into(PIODB.StackTable).Set(StackTable.FactoryID, factoryID).Set(StackTable.ResourceTypeID, (int)ResourceTypeIDs.Tree).Set(StackTable.Quantity, 100);
					#endregion

					yield return new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.X, 1).Set(BuildingTable.Y, 0).Set(BuildingTable.BuildingTypeID, BuildingTypeIDs.Factory).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
					yield return new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
					yield return new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, buildingID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Stockpile);

					yield return new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.X, 2).Set(BuildingTable.Y, 0).Set(BuildingTable.BuildingTypeID, BuildingTypeIDs.Factory).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
					yield return new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
					yield return new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, buildingID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Sawmill);

					#endregion

					#region create startup Worker
					yield return new Insert().Into(PIODB.WorkerTable).Set(WorkerTable.PlanetID, planetID).Set(WorkerTable.X, 0).Set(WorkerTable.Y, 1);
					yield return new SelectIdentity((result) => workerID = Convert.ToInt32(result));
					#endregion





					//yield return new Insert().Into(TaskTable)Set(TaskTable.WorkerID, factoryID).Set(TaskTable.ETA, DateTime.Now);


					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);
					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.FactoryTypeID, FactoryTypeIDs.Forest).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Stone).Set(MaterialTable.Quantity, 2);

					yield return new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.FactoryTypeID, FactoryTypeIDs.Stockpile).Set(MaterialTable.ResourceTypeID, ResourceTypeIDs.Wood).Set(MaterialTable.Quantity, 1);

					break;*/



			}
		}


	}

}
