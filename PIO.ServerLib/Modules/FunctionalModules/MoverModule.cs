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

		public Task[] BeginMoveTo(int WorkerID, int BuildingID)
		{
			Worker worker;
			Building building;
			List<Task> tasks;
			Task task;
			int dx, dy;
			int stepsX, stepsY;
			int step;
			DateTime now;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);
			building = AssertExists(() => buildingModule.GetBuilding(BuildingID), $"BuildingID={BuildingID}");

			if (worker.PlanetID!=building.PlanetID) Throw< PIOInvalidOperationException>(LogLevels.Warning, $"Worker is not in the same planet as building (WorkerID={WorkerID})");

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			if (worker.X < building.X) dx = 1; else dx = -1;
			if (worker.Y < building.Y) dy = 1; else dy = -1;
			stepsX = Math.Abs(worker.X - building.X);
			stepsY = Math.Abs(worker.Y - building.Y);

			tasks = new List<Task>();
			step = 0;now = DateTime.Now;
			for (int x = 0; x < stepsX; x++)
			{
				task = Try(() => taskModule.CreateTask(TaskTypeIDs.MoveTo, WorkerID, dx, 0, null, null, null, now.AddSeconds(step * 2))).OrThrow<PIOInternalErrorException>("Failed to create task");
				tasks.Add(task);
				step++;

			}
			for (int y = 0; y < stepsY; y++)
			{
				task = Try(() => taskModule.CreateTask(TaskTypeIDs.MoveTo, WorkerID, 0, dy, null, null, null, now.AddSeconds(step * 2))).OrThrow<PIOInternalErrorException>("Failed to create task");
				tasks.Add(task);
				step++;
			}
			OnTasksCreated(tasks.ToArray());

			return tasks.ToArray();
		}

		public Task[] BeginMoveTo(int WorkerID, int X, int Y)
		{
			Worker worker;
			List<Task> tasks;
			Task task;
			int dx, dy;
			int stepsX, stepsY;
			int step;
			DateTime now;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);


			Log(LogLevels.Information, $"Creating tasks (WorkerID={WorkerID})");

			if (worker.X < X) dx = 1; else dx = -1;
			if (worker.Y < Y) dy = 1; else dy = -1;
			stepsX = Math.Abs(worker.X - X);
			stepsY = Math.Abs(worker.Y - Y);

			tasks = new List<Task>();
			step = 0; now = DateTime.Now;
			for (int x = 0; x < stepsX; x++)
			{
				task = Try(() => taskModule.CreateTask(TaskTypeIDs.MoveTo, WorkerID, dx, 0, null, null, null, now.AddSeconds(step * 2))).OrThrow<PIOInternalErrorException>("Failed to create task");
				tasks.Add(task);
				step++;

			}
			for (int y = 0; y < stepsY; y++)
			{
				task = Try(() => taskModule.CreateTask(TaskTypeIDs.MoveTo, WorkerID, 0, dy, null, null, null, now.AddSeconds(step * 2))).OrThrow<PIOInternalErrorException>("Failed to create task");
				tasks.Add(task);
				step++;
			}
			OnTasksCreated(tasks.ToArray());

			return tasks.ToArray();
		}

		public void EndMoveTo(int WorkerID, int X, int Y)
		{
			Worker worker;
			int newX, newY;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID={WorkerID}");
			newX = worker.X + X;newY = worker.Y + Y;
			Log(LogLevels.Information, $"Updating worker (WorkerID={WorkerID}, X={newX}, Y={newY})");
			Try(() => workerModule.UpdateWorker(WorkerID,newX,newY)).OrThrow<PIOInternalErrorException>("Failed to update worker");
		}





	}
}
