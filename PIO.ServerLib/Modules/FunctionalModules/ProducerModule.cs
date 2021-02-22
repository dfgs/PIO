using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class ProducerModule : TaskGeneratorModule, IProducerModule
	{
		private IBuildingModule buildingModule;
		private IBuildingTypeModule buildingTypeModule;
		private IStackModule stackModule;
		private IIngredientModule ingredientModule;
		private IProductModule productModule;



		public ProducerModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IBuildingModule BuildingModule, IBuildingTypeModule BuildingTypeModule, IStackModule StackModule, IIngredientModule IngredientModule, IProductModule ProductModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule; this.buildingTypeModule = BuildingTypeModule; this.stackModule = StackModule; this.ingredientModule = IngredientModule;this.productModule = ProductModule; 
		}


		public Task BeginProduce(int WorkerID)
		{
			Building building;
			BuildingType buildingType;
			Worker worker;
			Ingredient[] ingredients;
			Product[] products;
			Task task;
			Stack stack;
			int quantity;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);

			building = AssertExists(() => buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");
			if (building.RemainingBuildSteps > 0) Throw< PIOInvalidOperationException>(LogLevels.Warning, $"Building is building (BuildingID={building.BuildingID})");

			buildingType = AssertExists(() => buildingTypeModule.GetBuildingType(building.BuildingTypeID), $"BuildingTypeID={building.BuildingTypeID}");
			if (!buildingType.IsFactory) Throw< PIOInvalidOperationException>(LogLevels.Warning, $"Building is not a factory (BuildingID={building.BuildingID})");

			ingredients = AssertExists(() => ingredientModule.GetIngredients(building.BuildingTypeID), $"BuildingTypeID={building.BuildingTypeID}");
			products=AssertExists(() => productModule.GetProducts(building.BuildingTypeID), $"BuildingTypeID={building.BuildingTypeID}");
			if (products.Length == 0)
			{
				Log(LogLevels.Warning, $"This building has no product (BuildingTypeID={building.BuildingTypeID})");
				return null;
			}

			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				quantity=Try(()=> stackModule.GetStackQuantity(building.BuildingID, ingredient.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to check stack quantity");
				if (quantity < ingredient.Quantity) Throw< PIONoResourcesException>(LogLevels.Warning, $"Not enough resources (BuildingID={building.BuildingID}, ResourceTypeID={ingredient.ResourceTypeID})");
			}

			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Consuming ingredient (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack=Try(() => stackModule.GetStack(building.BuildingID, ingredient.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to consume ingredient");
				stack.Quantity -= ingredient.Quantity;

				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}

			
			
			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.Produce, WorkerID, worker.X, worker.Y,null, null, null,  DateTime.Now.AddSeconds(products[0].Duration))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndProduce(int WorkerID)
		{
			Building building;
			Product[] products;
			Stack[] stacks;
			Stack stack;
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");


			building=AssertExists(() => buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");

			Log(LogLevels.Information, $"Get stacks (BuildingID={building.BuildingID})");
			stacks=Try(() => stackModule.GetStacks(building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");

			Log(LogLevels.Information, $"Get products (BuildingTypeID={building.BuildingTypeID})");
			products=Try(() => productModule.GetProducts(building.BuildingTypeID)).OrThrow<PIOInternalErrorException>("Failed to get products");
			if (products.Length == 0)
			{
				Log(LogLevels.Warning, $"This factory has no product (BuildingTypeID={building.BuildingTypeID})");
				return;
			}

			foreach (Product product in products)
			{
				Log(LogLevels.Information, $"Adding product (ResourceTypeID={product.ResourceTypeID}, Quantity={product.Quantity})");
				stack=stacks.FirstOrDefault(item => item.ResourceTypeID == product.ResourceTypeID);

				if (stack == null)
				{
					Try(() => stackModule.InsertStack(building.BuildingID, product.ResourceTypeID,product.Quantity )).OrThrow<PIOInternalErrorException>("Failed to insert stack");
				}
				else
				{
					stack.Quantity += product.Quantity;
					Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
				}
			}


		}

	}
}
