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
using PIO.Models.Exceptions;


namespace PIO.ServerLib.Modules
{
	public class ProducerModule : TaskGeneratorModule, IProducerModule
	{
		private IBuildingModule buildingModule;
		private IFactoryModule factoryModule;
		private IStackModule stackModule;
		private IIngredientModule ingredientModule;
		private IProductModule productModule;



		public ProducerModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IBuildingModule BuildingModule, IFactoryModule FactoryModule, IStackModule StackModule, IIngredientModule IngredientModule, IProductModule ProductModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule; this.factoryModule = FactoryModule;  this.stackModule = StackModule; this.ingredientModule = IngredientModule;this.productModule = ProductModule; 
		}


		public Task BeginProduce(int WorkerID)
		{
			Building building;
			Factory factory;
			Worker worker;
			Ingredient[] ingredients;
			Product[] products;
			Task task;
			Stack stack;
			int quantity;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);

			factory = AssertExists(() => factoryModule.GetFactory(worker.PlanetID, worker.X, worker.Y), $"X = {worker.X}, Y = {worker.Y}");
			building = AssertExists(() => buildingModule.GetBuilding(factory.BuildingID), $"BuildingID = {factory.BuildingID}");
			if (building.RemainingBuildSteps>0)
			{
				Log(LogLevels.Warning, $"Factory is building (FactoryID={factory.FactoryID})");
				throw new PIOInvalidOperationException($"Factory is building (FactoryID={factory.FactoryID})", null, ID, ModuleName, "BeginProduce");
			}


			ingredients = AssertExists(() => ingredientModule.GetIngredients(factory.FactoryTypeID), $"FactoryTypeID = {factory.FactoryTypeID}");
			products = AssertExists(() => productModule.GetProducts(factory.FactoryTypeID), $"FactoryTypeID = {factory.FactoryTypeID}");
			if (products.Length == 0)
			{
				Log(LogLevels.Warning, $"This factory has no product (FactoryTypeID={factory.FactoryTypeID})");
				return null;
			}

			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				quantity = Try(()=> stackModule.GetStackQuantity(factory.FactoryID, ingredient.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to check stack quantity");
				if (quantity < ingredient.Quantity)
				{
					Log(LogLevels.Warning, $"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={ingredient.ResourceTypeID})");
					throw new PIONoResourcesException($"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={ingredient.ResourceTypeID})", null, ID, ModuleName, "BeginProduce");
				}
			}

			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Consuming ingredient (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack = Try(() => stackModule.GetStack(factory.FactoryID, ingredient.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to consume ingredient");
				stack.Quantity -= ingredient.Quantity;

				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}

			
			
			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.Produce, WorkerID, null, null,null, null, null, null, DateTime.Now.AddSeconds(products[0].Duration))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndProduce(int WorkerID)
		{
			Factory factory;
			Product[] products;
			Stack[] stacks;
			Stack stack;
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID = {WorkerID}");


			factory = AssertExists(() => factoryModule.GetFactory(worker.PlanetID, worker.X, worker.Y), $"X = {worker.X}, Y = {worker.Y}");

			Log(LogLevels.Information, $"Get stacks (FactoryID={factory.FactoryID})");
			stacks = Try(() => stackModule.GetStacks(factory.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");

			Log(LogLevels.Information, $"Get products (FactoryTypeID={factory.FactoryTypeID})");
			products = Try(() => productModule.GetProducts(factory.FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get products");
			if (products.Length == 0)
			{
				Log(LogLevels.Warning, $"This factory has no product (FactoryTypeID={factory.FactoryTypeID})");
				return;
			}

			foreach (Product product in products)
			{
				Log(LogLevels.Information, $"Adding product (ResourceTypeID={product.ResourceTypeID}, Quantity={product.Quantity})");
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == product.ResourceTypeID);

				if (stack == null)
				{
					Try(() => stackModule.InsertStack(factory.FactoryID,product.ResourceTypeID,product.Quantity )).OrThrow<PIOInternalErrorException>("Failed to insert stack");
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
