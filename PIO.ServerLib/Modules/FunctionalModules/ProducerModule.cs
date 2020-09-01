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
	public class ProducerModule : FunctionalModule, IProducerModule
	{
		private ISchedulerModule schedulerModule;

		private IFactoryModule factoryModule;
		private IWorkerModule workerModule;
		private IStackModule stackModule;
		private IIngredientModule ingredientModule;
		private ITaskModule taskModule;

		public ProducerModule(ILogger Logger,ISchedulerModule SchedulerModule, IFactoryModule FactoryModule,IWorkerModule WorkerModule, IStackModule StackModule, IIngredientModule IngredientModule,ITaskModule TaskModule) : base(Logger)
		{
			this.schedulerModule = SchedulerModule;
			this.factoryModule = FactoryModule; this.workerModule = WorkerModule; this.stackModule = StackModule; this.ingredientModule = IngredientModule;this.taskModule = TaskModule;
		}

		public Task Produce(int WorkerID,int FactoryID)
		{
			Factory factory;
			Worker worker;
			Ingredient[] ingredients;
			Stack[] stacks;
			Stack stack;
			Task task;

			LogEnter();

			Log(LogLevels.Information, $"Get factory (FactoryID={FactoryID})");
			factory = Try(() => factoryModule.GetFactory(FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get factory");

			if (factory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={FactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={FactoryID})", null, ID, ModuleName, "Produce");
			}

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");

			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "Produce");
			}


			Log(LogLevels.Information, $"Get ingredients (FactoryTypeID={factory.FactoryTypeID})");
			ingredients = Try(() => ingredientModule.GetIngredients(factory.FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get ingredients");

			Log(LogLevels.Information, $"Get stacks (FactoryID={factory.FactoryID})");
			stacks = Try(() => stackModule.GetStacks(factory.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");


			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == ingredient.ResourceTypeID);
				if ((stack == null) || (stack.Quantity < ingredient.Quantity))
				{
					Log(LogLevels.Warning, $"Not enough resources (FactoryID={FactoryID}, ResourceTypeID={ingredient.ResourceTypeID})");
					throw new PIONoResourcesException($"Not enough resources (FactoryID={FactoryID}, ResourceTypeID={ingredient.ResourceTypeID})", null, ID, ModuleName, "Produce");
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
			task=Try(() => taskModule.InsertTask((int)TaskTypeIDs.Produce, WorkerID,DateTime.Now.AddMinutes(5))).OrThrow<PIOInternalErrorException>("Failed to create task");

			Log(LogLevels.Information, $"Scheduling task (TaskID={task.TaskID})");
			Try(()=>schedulerModule.Add(task)).OrThrow<PIOInternalErrorException>("Failed to schedule task");

			return task;
		}



	}
}
