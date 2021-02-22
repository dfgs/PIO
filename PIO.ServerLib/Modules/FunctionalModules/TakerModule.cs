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
	public class TakerModule : TaskGeneratorModule, ITakerModule
	{

		private static int carriedQuantity = 1;

		private IBuildingModule buildingModule;
		private IStackModule stackModule;



		public TakerModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IBuildingModule BuildingModule, IStackModule StackModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule; this.stackModule = StackModule;
		}

		public Task BeginTake(int WorkerID,ResourceTypeIDs ResourceTypeID)
		{
			Building building;
			Worker worker;
			Stack stack;
			Task task;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);

			Log(LogLevels.Information, $"Check if worker is not carrying item (WorkerID={worker.WorkerID})");
			if (worker.ResourceTypeID!= null)  Throw< PIOInvalidOperationException>(LogLevels.Warning, $"Worker is already carrying item (WorkerID={worker.WorkerID}, ResourceTypeID={worker.ResourceTypeID})");

			building = AssertExists(() => buildingModule.GetBuilding(worker.PlanetID, worker.X, worker.Y), $"X={worker.X}, Y={worker.Y}");
			Log(LogLevels.Information, $"Check stack quantity (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID})");
			stack = Try(() => stackModule.GetStack(building.BuildingID, ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to get stack");
			if ((stack == null) || (stack.Quantity < carriedQuantity)) Throw<PIONoResourcesException>(LogLevels.Warning, $"Not enough resources (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID})");

			Log(LogLevels.Information, $"Consuming resource (BuildingID={building.BuildingID}, ResourceTypeID={ResourceTypeID}, Quantity={carriedQuantity})");
			stack.Quantity -= carriedQuantity;
			Try(() => stackModule.UpdateStack(stack.StackID, stack.Quantity)).OrThrow<PIOInternalErrorException>("Failed to update stack");


			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task = Try(() => taskModule.CreateTask(TaskTypeIDs.Take, WorkerID, worker.X, worker.Y, null, ResourceTypeID, null, DateTime.Now.AddSeconds(5))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndTake(int WorkerID,ResourceTypeIDs ResourceTypeID)
		{
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");

			Log(LogLevels.Information, $"Updating worker (WorkerID={WorkerID})");
			Try(() => workerModule.UpdateWorker(WorkerID, ResourceTypeID)).OrThrow<PIOInternalErrorException>("Failed to update worker");
		}





	}
}
