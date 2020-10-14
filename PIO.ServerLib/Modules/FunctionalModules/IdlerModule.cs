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
	public class IdlerModule : TaskGeneratorModule, IIdlerModule
	{

		private IWorkerModule workerModule;



		public IdlerModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule) : base(Logger,TaskModule)
		{
			this.workerModule = WorkerModule;
		}

		public Task BeginIdle(int WorkerID,int Duration)
		{
			Task task;
			Worker worker;

			LogEnter();
			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");
			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "BeginMoveTo");
			}

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task = Try(() => taskModule.InsertTask(TaskTypeIDs.Idle, WorkerID,null, null,GetLastETA(WorkerID).AddSeconds(Duration))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndIdle(int WorkerID)
		{
			Worker worker;

			LogEnter();

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");
			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "EndIdleTo");
			}

		}





	}
}
