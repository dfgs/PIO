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
	public class StorerModule : TaskGeneratorModule, IStorerModule
	{

		private static int carriedQuantity = 1;

		private IBuildingModule buildingModule;
		private IStackModule stackModule;



		public StorerModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IBuildingModule BuildingModule, IStackModule StackModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule; this.stackModule = StackModule;
		}

		public Task BeginStore(int WorkerID)
		{
			Building building;
			Worker worker;
			Task task;
			ResourceTypeIDs? resourceTypeID;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);

			Log(LogLevels.Information, $"Check if worker is carrying item (WorkerID={worker.WorkerID})");
			if (worker.ResourceTypeID== null)  Throw< PIOInvalidOperationException>(LogLevels.Warning, $"Worker is not carrying item (WorkerID={worker.WorkerID})");
				
			building = AssertExists(() => buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");

			resourceTypeID = worker.ResourceTypeID;
			Log(LogLevels.Information, $"Updating worker (WorkerID={WorkerID})");
			Try(() => workerModule.UpdateWorker(WorkerID, null)).OrThrow<PIOInternalErrorException>("Failed to update worker");

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task = Try(() => taskModule.CreateTask(TaskTypeIDs.Store, WorkerID, worker.X, worker.Y, null, resourceTypeID, null, DateTime.Now.AddSeconds(5))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndStore(int WorkerID, ResourceTypeIDs ResourceTypeID)
		{
			Building building;
			Stack stack;
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");

			building = AssertExists(() => buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");

			Log(LogLevels.Information, $"Get stack (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID})");
			stack = Try(() => stackModule.GetStack(building.BuildingID, ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to get stack");

			Log(LogLevels.Information, $"Adding resource (ResourceTypeID={ResourceTypeID}, Quantity={carriedQuantity})");
			if (stack == null)
			{
				Try(() => stackModule.InsertStack(building.BuildingID, ResourceTypeID, carriedQuantity)).OrThrow<PIOInternalErrorException>("Failed to insert stack");
			}
			else
			{
				stack.Quantity += carriedQuantity;
				Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");
			}


		}





	}
}
