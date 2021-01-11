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
	public class MoverModule : TaskGeneratorModule, IMoverModule
	{

		private IBuildingModule buildingModule;



		public MoverModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IBuildingModule BuildingModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.buildingModule = BuildingModule;
		}

		public Task BeginMoveTo(int WorkerID, int BuildingID)
		{
			Worker worker;
			Building building;
			Task task;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);
			building = AssertExists(() => buildingModule.GetBuilding(BuildingID), $"BuildingID={BuildingID}");

			if (worker.PlanetID!=building.PlanetID)
			{
				Log(LogLevels.Warning, $"Worker is not in the same planet as building (WorkerID={WorkerID})");
				throw new PIOInvalidOperationException($"Worker is not in the same planet as building (WorkerID={WorkerID})", null, ID, ModuleName, "BeginMoveTo");
			}

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.MoveTo, WorkerID, building.X, building.Y, null, null, null, null, DateTime.Now.AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public Task BeginMoveTo(int WorkerID, int X, int Y)
		{
			Worker worker;
			Task task;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);


			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task=Try(() => taskModule.CreateTask(TaskTypeIDs.MoveTo, WorkerID, X, Y, null, null, null, null, DateTime.Now.AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndMoveTo(int WorkerID, int X, int Y)
		{
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");

			Log(LogLevels.Information, $"Updating worker (WorkerID={WorkerID}, X={X}, Y={Y})");
			Try(() => workerModule.UpdateWorker(WorkerID,X,Y)).OrThrow<PIOInternalErrorException>("Failed to update worker");
		}





	}
}
