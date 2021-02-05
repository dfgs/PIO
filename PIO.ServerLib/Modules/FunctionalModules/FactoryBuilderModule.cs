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
	public class FactoryBuilderModule : TaskGeneratorModule, IFactoryBuilderModule
	{
		private IBuildingModule buildingModule;
		private IFactoryModule factoryModule;
		private IFactoryTypeModule factoryTypeModule;
		private IStackModule stackModule;
		private IMaterialModule materialModule;

		public FactoryBuilderModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule,  
			IBuildingModule BuildingModule, IFactoryModule FactoryModule, IFactoryTypeModule FactoryTypeModule,
			IStackModule StackModule,IMaterialModule MaterialModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule; this.factoryModule = FactoryModule; this.factoryTypeModule = FactoryTypeModule;
			this.stackModule = StackModule;this.materialModule = MaterialModule;

		}


		public Task BeginCreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID)
		{
			FactoryType factoryType;
			Worker worker;
			Building building;
			Task task;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");

			Log(LogLevels.Information, $"Checking if worker is idle (WorkerID={WorkerID})");
			task=Try(() => taskModule.GetLastTask(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get last task");
			if (task != null)
			{
				Log(LogLevels.Warning, $"Worker is not free (WorkerID={WorkerID})");
				throw new PIOInvalidOperationException($"Worker is not free (WorkerID={WorkerID})", null, ID, ModuleName, "BeginCreateBuilding");
			}

			Log(LogLevels.Information, $"Checking if position is free (X={worker.X}, Y={worker.Y})");
			building=Try(() => buildingModule.GetBuilding(worker.PlanetID,worker.X, worker.Y)).OrThrow<PIOInternalErrorException>("Failed to get building at position");
			if (building!=null)
			{
				Log(LogLevels.Warning, $"Current position is not free (X={worker.X}, Y={worker.Y})");
				throw new PIOInvalidOperationException($"Current position is not free (X={worker.X}, Y={worker.Y})", null, ID, ModuleName, "BeginCreateBuilding");
			}
			factoryType=AssertExists(() => factoryTypeModule.GetFactoryType(FactoryTypeID), $"FactoryTypeID={FactoryTypeID}");

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.CreateBuilding, WorkerID, worker.X, worker.Y, null, null, FactoryTypeID, DateTime.Now.AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndCreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID)
		{
			FactoryType factoryType;
			Factory factory;
			Worker worker;

			LogEnter();

			Log(LogLevels.Information, $"End create building (FactoryTypeID={FactoryTypeID})");

			worker=AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");
			factoryType=AssertExists(() => factoryTypeModule.GetFactoryType(FactoryTypeID), $"FactoryTypeID={FactoryTypeID}");

			//Log(LogLevels.Information, $"Creating building");
			//building=Try(() => buildingModule.CreateBuilding(worker.PlanetID, worker.X, worker.Y, factoryType.BuildSteps)).OrThrow<PIOInternalErrorException>("Failed to create building");

			Log(LogLevels.Information, $"Creating factory (FactoryTypeID={FactoryTypeID})");
			factory=Try(() => factoryModule.CreateFactory(worker.PlanetID, worker.X, worker.Y, factoryType.BuildSteps,FactoryTypeID )).OrThrow<PIOInternalErrorException>("Failed to create factory");

		}

		public Task BeginBuild(int WorkerID)
		{
			//Building building;
			Factory factory;
			Worker worker;
			Material[] materials;
			Stack stack;
			int quantity;
			Task task;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);



			factory = AssertExists(() => factoryModule.GetFactory(worker.PlanetID, worker.X,worker.Y), $"X={worker.X}, Y={worker.Y}");
			//building=AssertExists(() => buildingModule.GetBuilding(factory.BuildingID), $"BuildingID={factory.BuildingID}");
			if (factory.RemainingBuildSteps==0)
			{
				Log(LogLevels.Warning, $"Factory is already build (FactoryID={factory.FactoryID})");
				throw new PIOInvalidOperationException($"Factory is already build (FactoryID={factory.FactoryID})", null, ID, ModuleName, "BeginBuild");
			}

			materials=AssertExists(() => materialModule.GetMaterials(factory.FactoryTypeID), $"FactoryTypeID={factory.FactoryTypeID}");

			foreach (Material material in materials)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={material.ResourceTypeID}, Quantity={material.Quantity})");
				quantity=Try(() => stackModule.GetStackQuantity(factory.BuildingID, material.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to check stack quantity");
				if (quantity < material.Quantity)
				{
					Log(LogLevels.Warning, $"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={material.ResourceTypeID})");
					throw new PIONoResourcesException($"Not enough resources (FactoryID={factory.FactoryID}, ResourceTypeID={material.ResourceTypeID})", null, ID, ModuleName, "BeginBuild");
				}
			}

			foreach (Material material in materials)
			{
				Log(LogLevels.Information, $"Consuming ingredient (ResourceTypeID={material.ResourceTypeID}, Quantity={material.Quantity})");
				stack=Try(() => stackModule.GetStack(factory.BuildingID, material.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to consume ingredient");
				stack.Quantity -= material.Quantity;

				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.Build, WorkerID, worker.X, worker.Y, null, null, null, DateTime.Now.AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndBuild(int WorkerID)
		{
			Building building;
			Factory factory;
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");
			factory=AssertExists(() => factoryModule.GetFactory(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");
			building=AssertExists(() => buildingModule.GetBuilding(factory.BuildingID), $"BuildingID={factory.BuildingID}");

			building.RemainingBuildSteps -= 1;

			Log(LogLevels.Information, $"Updating remaining steps (BuildingID={building.BuildingID})");
			Try(() => buildingModule.UpdateBuilding(building.BuildingID, building.RemainingBuildSteps)).OrThrow<PIOInternalErrorException>("Failed to update building");

		}



	}
}
