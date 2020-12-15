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
	public class IdlerModule : TaskGeneratorModule, IIdlerModule
	{




		public IdlerModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.workerModule = WorkerModule;
		}

		public Task BeginIdle(int WorkerID,int Duration)
		{
			Task task;
			Worker worker;

			LogEnter();

			worker = AssertWorkerIsIdle(WorkerID);


			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task = Try(() => taskModule.CreateTask(TaskTypeIDs.Idle, WorkerID, null, null,null, null, null, null, DateTime.Now.AddSeconds(Duration))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndIdle(int WorkerID)
		{
			Worker worker;

			LogEnter();

			worker = AssertExists(() => workerModule.GetWorker(WorkerID), $"WorkerID = {WorkerID}");

		}





	}
}
