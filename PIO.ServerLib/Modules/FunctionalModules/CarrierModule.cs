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
	public class CarrierModule : TaskGeneratorModule, ICarrierModule
	{

		private static int carriedQuantity = 1;

		private IBuildingModule buildingModule;
		private IStackModule stackModule;



		public CarrierModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IBuildingModule BuildingModule, IStackModule StackModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule;  this.stackModule = StackModule;  
		}


		public Task BeginCarryTo(int WorkerID, int TargetBuildingID, ResourceTypeIDs ResourceTypeID)
		{
			Building building,targetBuilding;
			Worker worker;
			Stack stack;
			Task task;
	
			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);


			building = AssertExists(() => buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");

			targetBuilding = AssertExists(() => buildingModule.GetBuilding(TargetBuildingID), $"BuildingID={TargetBuildingID}");

			Log(LogLevels.Information, $"Check stack quantity (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID})");
			stack = Try(() => stackModule.GetStack(building.BuildingID, ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to get stack");
			if ((stack==null) || (stack.Quantity < carriedQuantity))
			{
				Log(LogLevels.Warning, $"Not enough resources (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID})");
				throw new PIONoResourcesException($"Not enough resources (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID})", null, ID, ModuleName, "BeginCarryTo");
			}

			Log(LogLevels.Information, $"Consuming resource (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID}, Quantity={carriedQuantity})");
			stack.Quantity -= carriedQuantity;
			Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			
			
			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.CarryTo, WorkerID, null, null,null, TargetBuildingID, ResourceTypeID,null, DateTime.Now.AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndCarryTo(int WorkerID, int TargetBuildingID, ResourceTypeIDs ResourceTypeID)
		{
			Building targetBuilding;
			Stack stack;
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID = {WorkerID}");

			targetBuilding = AssertExists(() => buildingModule.GetBuilding(TargetBuildingID), $"BuildingID={TargetBuildingID}");

			Log(LogLevels.Information, $"Get stack (FactoryID={TargetBuildingID}, ResourceTypeID={ResourceTypeID})");
			stack = Try(() => stackModule.GetStack(TargetBuildingID, ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to get stack");

			Log(LogLevels.Information, $"Adding resource (ResourceTypeID={ResourceTypeID}, Quantity={carriedQuantity})");
			if (stack == null)
			{
				Try(() => stackModule.InsertStack(TargetBuildingID, ResourceTypeID, carriedQuantity)).OrThrow<PIOInternalErrorException>("Failed to insert stack");
			}
			else
			{
				stack.Quantity += carriedQuantity;
				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}

			Log(LogLevels.Information, $"Updating worker (WorkerID={WorkerID})");
			Try(() => workerModule.UpdateWorker(WorkerID, targetBuilding.X, targetBuilding.Y)).OrThrow<PIOInternalErrorException>("Failed to update worker");


		}

	}
}
