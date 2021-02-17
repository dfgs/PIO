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
	public class BuilderModule : TaskGeneratorModule, IBuilderModule
	{
		private IBuildingModule buildingModule;
		private IFactoryModule factoryModule;
		private IFarmModule farmModule;
		private IFactoryTypeModule factoryTypeModule;
		private IFarmTypeModule farmTypeModule;
		private IStackModule stackModule;
		private IMaterialModule materialModule;

		public BuilderModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule,  
			IBuildingModule BuildingModule, IFactoryModule FactoryModule, IFactoryTypeModule FactoryTypeModule,
			IFarmModule FarmModule,IFarmTypeModule FarmTypeModule,
			IStackModule StackModule,IMaterialModule MaterialModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule; 
			this.factoryModule = FactoryModule; this.factoryTypeModule = FactoryTypeModule;
			this.farmModule = FarmModule;this.farmTypeModule = FarmTypeModule;
			this.stackModule = StackModule;this.materialModule = MaterialModule;

		}


		public Task BeginCreateBuilding(int WorkerID, FactoryTypeIDs? FactoryTypeID, FarmTypeIDs? FarmTypeID)
		{
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


			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.CreateBuilding, WorkerID, worker.X, worker.Y, null, null, FactoryTypeID,FarmTypeID, DateTime.Now.AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndCreateBuilding(int WorkerID, FactoryTypeIDs? FactoryTypeID, FarmTypeIDs? FarmTypeID)
		{
			FactoryType factoryType;
			FarmType farmType;
			Factory factory;
			Farm farm;
			Worker worker;

			LogEnter();
			
			Log(LogLevels.Information, $"End create building (FactoryTypeID={FactoryTypeID})");

			worker=AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");
			if (FactoryTypeID.HasValue)
			{
				factoryType = AssertExists(() => factoryTypeModule.GetFactoryType(FactoryTypeID.Value), $"FactoryTypeID={FactoryTypeID}");
				Log(LogLevels.Information, $"Creating factory (FactoryTypeID={FactoryTypeID})");
				factory = Try(() => factoryModule.CreateFactory(worker.PlanetID, worker.X, worker.Y, factoryType.BuildSteps, FactoryTypeID.Value)).OrThrow<PIOInternalErrorException>("Failed to create factory");
			}
			else if (FarmTypeID.HasValue)
			{
				farmType = AssertExists(() => farmTypeModule.GetFarmType(FarmTypeID.Value), $"FarmTypeID={FarmTypeID}");
				Log(LogLevels.Information, $"Creating farm (FarmTypeID={FarmTypeID})");
				farm = Try(() => farmModule.CreateFarm(worker.PlanetID, worker.X, worker.Y, farmType.BuildSteps, FarmTypeID.Value)).OrThrow<PIOInternalErrorException>("Failed to create farm");
			}
			else
			{
				Log(LogLevels.Error, $"No valid building type provided");
				throw new PIOInternalErrorException($"No valid building type provided", null, ID, ModuleName, "EndCreateBuilding");
			}

		}

		public Task BeginBuild(int WorkerID)
		{
			//Building building;
			Building building;
			FactoryType factoryType;
			FarmType farmType;
			Worker worker;
			Material[] materials;
			Stack stack;
			int quantity;
			Task task;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);



			building = AssertExists(() =>
				(Building)factoryModule.GetFactory(worker.PlanetID, worker.X, worker.Y) ?? (Building)farmModule.GetFarm(worker.PlanetID, worker.X, worker.Y),
			$"X={worker.X}, Y={worker.Y}");

			if (building is Factory factory)
			{
				if (factory.RemainingBuildSteps == 0)
				{
					Log(LogLevels.Warning, $"Factory is already build (FactoryID={factory.FactoryID})");
					throw new PIOInvalidOperationException($"Factory is already build (FactoryID={factory.FactoryID})", null, ID, ModuleName, "BeginBuild");
				}

				Log(LogLevels.Information, $"Get FactoryType (FactoryTypeID={factory.FactoryTypeID})");
				factoryType = Try(() => factoryTypeModule.GetFactoryType(factory.FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get FactoryType");

				materials = AssertExists(() => materialModule.GetMaterials(factoryType.MaterialSetID), $"FactoryTypeID={factory.FactoryTypeID}");
			}
			else if (building is Farm farm)
			{
				if (farm.RemainingBuildSteps == 0)
				{
					Log(LogLevels.Warning, $"Farm is already build (FarmID={farm.FarmID})");
					throw new PIOInvalidOperationException($"Farm is already build (FarmID={farm.FarmID})", null, ID, ModuleName, "BeginBuild");
				}

				Log(LogLevels.Information, $"Get FarmType (FarmTypeID={farm.FarmTypeID})");
				farmType = Try(() => farmTypeModule.GetFarmType(farm.FarmTypeID)).OrThrow<PIOInternalErrorException>("Failed to get FarmType");

				materials = AssertExists(() => materialModule.GetMaterials(farmType.MaterialSetID), $"FarmTypeID={farm.FarmTypeID}");

			}
			else
			{
				Log(LogLevels.Error, $"No valid building type provided");
				throw new PIOInternalErrorException($"No valid building type provided", null, ID, ModuleName, "BeginBuild");
			}

			foreach (Material material in materials)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={material.ResourceTypeID}, Quantity={material.Quantity})");
				quantity=Try(() => stackModule.GetStackQuantity(building.BuildingID, material.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to check stack quantity");
				if (quantity < material.Quantity)
				{
					Log(LogLevels.Warning, $"Not enough resources (BuildingID={building.BuildingID}, ResourceTypeID={material.ResourceTypeID})");
					throw new PIONoResourcesException($"Not enough resources (BuildingID={building.BuildingID}, ResourceTypeID={material.ResourceTypeID})", null, ID, ModuleName, "BeginBuild");
				}
			}

			foreach (Material material in materials)
			{
				Log(LogLevels.Information, $"Consuming ingredient (ResourceTypeID={material.ResourceTypeID}, Quantity={material.Quantity})");
				stack=Try(() => stackModule.GetStack(building.BuildingID, material.ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to consume ingredient");
				stack.Quantity -= material.Quantity;

				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.Build, WorkerID, worker.X, worker.Y, null, null, null,null, DateTime.Now.AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndBuild(int WorkerID)
		{
			Building building;
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");
			building=AssertExists(() =>  buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y)  , $"X={worker.X}, Y={worker.Y}");

			building.RemainingBuildSteps -= 1;

			Log(LogLevels.Information, $"Updating remaining steps (BuildingID={building.BuildingID})");
			Try(() => buildingModule.UpdateBuilding(building.BuildingID, building.RemainingBuildSteps)).OrThrow<PIOInternalErrorException>("Failed to update building");

		}



	}
}
