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
	public class HarvesterModule : TaskGeneratorModule, IHarvesterModule
	{
		private IBuildingModule buildingModule;
		private IBuildingTypeModule buildingTypeModule;
		private IStackModule stackModule;
		private IProductModule productModule;



		public HarvesterModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IBuildingModule BuildingModule, IBuildingTypeModule BuildingTypeModule, IStackModule StackModule, IProductModule ProductModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule; this.buildingTypeModule = BuildingTypeModule; this.stackModule = StackModule; this.productModule = ProductModule; 
		}


		public Task BeginHarvest(int WorkerID)
		{
			Building building;
			BuildingType buildingType;
			Worker worker;
			Product[] products;
			Task task;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);

			building = AssertExists(() => buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");
			if (building.RemainingBuildSteps > 0)
			{
				Log(LogLevels.Warning, $"Building is building (BuildingID={building.BuildingID})");
				throw new PIOInvalidOperationException($"Building is building (BuildingID={building.BuildingID})", null, ID, ModuleName, "BeginHarveste");
			}

			buildingType = AssertExists(() => buildingTypeModule.GetBuildingType(building.BuildingTypeID), $"BuildingTypeID={building.BuildingTypeID}");
			if (!buildingType.IsFarm)
			{
				Log(LogLevels.Warning, $"Building is not a farm (BuildingID={building.BuildingID})");
				throw new PIOInvalidOperationException($"Building is not a factory (BuildingID={building.BuildingID})", null, ID, ModuleName, "BeginHarveste");
			}


			products=AssertExists(() => productModule.GetProducts(building.BuildingTypeID), $"BuildingTypeID={building.BuildingTypeID}");
			if (products.Length == 0)
			{
				Log(LogLevels.Warning, $"This building has no product (BuildingTypeID={building.BuildingTypeID})");
				return null;
			}

				
			
			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.Harvest, WorkerID, worker.X, worker.Y,null, null, null,  DateTime.Now.AddSeconds(products[0].Duration))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndHarvest(int WorkerID)
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
