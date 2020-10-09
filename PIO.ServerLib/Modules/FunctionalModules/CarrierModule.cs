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
	public class CarrierModule : TaskGeneratorModule, ICarrierModule
	{

		private static int quantity = 1;

		private IFactoryModule factoryModule;
		private IWorkerModule workerModule;
		private IStackModule stackModule;



		public CarrierModule(ILogger Logger, ITaskModule TaskModule,IFactoryModule FactoryModule,IWorkerModule WorkerModule, IStackModule StackModule) : base(Logger,TaskModule)
		{
			 this.factoryModule = FactoryModule; this.workerModule = WorkerModule; this.stackModule = StackModule;  
		}


		public Task BeginCarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
			Factory factory,targetFactory;
			Worker worker;
			Stack[] stacks;
			Stack stack;
			Task task;

			LogEnter();

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");

			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "BeginCarry");
			}

			Log(LogLevels.Information, $"Get factory (FactoryID={worker.FactoryID})");
			factory = Try(() => factoryModule.GetFactory(worker.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get factory");

			/*if (factory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={worker.FactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={worker.FactoryID})", null, ID, ModuleName, "BeginProduce");
			}*/

			Log(LogLevels.Information, $"Get target factory (FactoryID={TargetFactoryID})");
			targetFactory = Try(() => factoryModule.GetFactory(TargetFactoryID)).OrThrow<PIOInternalErrorException>("Failed to get target factory");
			if (targetFactory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={TargetFactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={TargetFactoryID})", null, ID, ModuleName, "BeginCarry");
			}


			Log(LogLevels.Information, $"Get stacks (FactoryID={factory.FactoryID})");
			stacks = Try(() => stackModule.GetStacks(factory.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");

			Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ResourceTypeID}, Quantity={quantity})");
			stack = stacks.FirstOrDefault(item => item.ResourceTypeID == ResourceTypeID);
			if ((stack == null) || (stack.Quantity < quantity))
			{
				Log(LogLevels.Warning, $"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={ResourceTypeID})");
				throw new PIONoResourcesException($"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={ResourceTypeID})", null, ID, ModuleName, "BeginCarry");
			}
			Log(LogLevels.Information, $"Taking resource (ResourceTypeID={ResourceTypeID}, Quantity={quantity})");
			stack.Quantity -= quantity;
			Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			
			
			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.InsertTask(TaskTypeIDs.CarryTo, WorkerID, TargetFactoryID,ResourceTypeID, GetLastETA(WorkerID).AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndCarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
			Factory targetFactory;
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


			Log(LogLevels.Information, $"Get target factory (FactoryID={TargetFactoryID})");
			targetFactory = Try(() => factoryModule.GetFactory(TargetFactoryID)).OrThrow<PIOInternalErrorException>("Failed to get target factory");
			if (targetFactory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={TargetFactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={TargetFactoryID})", null, ID, ModuleName, "EndMoveTo");
			}

			Log(LogLevels.Information, $"Get stacks (FactoryID={TargetFactoryID})");
			stacks = Try(() => stackModule.GetStacks(TargetFactoryID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");

			Log(LogLevels.Information, $"Adding resource (ResourceTypeID={ResourceTypeID}, Quantity={quantity})");
			stack = stacks.FirstOrDefault(item => item.ResourceTypeID == ResourceTypeID);

			if (stack == null)
			{
				Try(() => stackModule.InsertStack(TargetFactoryID, ResourceTypeID,quantity )).OrThrow<PIOInternalErrorException>("Failed to insert stack");
			}
			else
			{
				stack.Quantity += quantity;
				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}

			Log(LogLevels.Information, $"Updating worker (WorkerID={WorkerID})");
			Try(() => workerModule.UpdateWorker(WorkerID, TargetFactoryID)).OrThrow<PIOInternalErrorException>("Failed to update worker");


		}

	}
}
