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

		private IFactoryModule factoryModule;
		private IWorkerModule workerModule;
		private IStackModule stackModule;
		private IIngredientModule ingredientModule;
		private IProductModule productModule;



		public ProducerModule(ILogger Logger, ITaskModule TaskModule,IFactoryModule FactoryModule,IWorkerModule WorkerModule, IStackModule StackModule, IIngredientModule IngredientModule, IProductModule ProductModule) : base(Logger,TaskModule)
		{
			 this.factoryModule = FactoryModule; this.workerModule = WorkerModule; this.stackModule = StackModule; this.ingredientModule = IngredientModule;this.productModule = ProductModule; 
		}


		public Task BeginProduce(int WorkerID)
		{
			Factory factory;
			Worker worker;
			Ingredient[] ingredients;
			Product[] products;
			Stack[] stacks;
			Stack stack;
			Task task;

			LogEnter();

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");

			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "BeginProduce");
			}

			Log(LogLevels.Information, $"Get factory (FactoryID={worker.FactoryID})");
			factory = Try(() => factoryModule.GetFactory(worker.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get factory");

			/*if (factory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={worker.FactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={worker.FactoryID})", null, ID, ModuleName, "BeginProduce");
			}*/


			Log(LogLevels.Information, $"Get ingredients (FactoryTypeID={factory.FactoryTypeID})");
			ingredients = Try(() => ingredientModule.GetIngredients(factory.FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get ingredients");

			Log(LogLevels.Information, $"Get products (FactoryTypeID={factory.FactoryTypeID})");
			products = Try(() => productModule.GetProducts(factory.FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get products");
			if (products.Length==0)
			{
				Log(LogLevels.Warning, $"This factory has no product (FactoryTypeID={factory.FactoryTypeID})");
				return null;
			}

			Log(LogLevels.Information, $"Get stacks (FactoryID={factory.FactoryID})");
			stacks = Try(() => stackModule.GetStacks(factory.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");


			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == ingredient.ResourceTypeID);
				if ((stack == null) || (stack.Quantity < ingredient.Quantity))
				{
					Log(LogLevels.Warning, $"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={ingredient.ResourceTypeID})");
					throw new PIONoResourcesException($"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={ingredient.ResourceTypeID})", null, ID, ModuleName, "BeginProduce");
				}
			}

			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Consuming ingredient (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == ingredient.ResourceTypeID);
				stack.Quantity -= ingredient.Quantity;

				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}

			
			
			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.InsertTask((int)TaskTypeIDs.Produce, WorkerID, null, null,GetLastETA(WorkerID).AddSeconds(products[0].Duration))).OrThrow<PIOInternalErrorException>("Failed to create task");

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

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");

			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "BeginProduce");
			}


			Log(LogLevels.Information, $"Get factory (FactoryID={worker.FactoryID})");
			factory = Try(() => factoryModule.GetFactory(worker.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get factory");

			/*if (factory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={FactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={FactoryID})", null, ID, ModuleName, "BeginProduce");
			}*/

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
