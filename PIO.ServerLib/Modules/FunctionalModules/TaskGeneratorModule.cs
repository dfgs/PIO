using LogLib;
using PIO.Models.Modules;
using PIO.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PIO.Models;

namespace PIO.ServerLib.Modules
{
	public abstract class TaskGeneratorModule : FunctionalModule, ITaskGeneratorModule
	{
		public event TaskCreatedHandler TaskCreated;
		protected ITaskModule taskModule;
		protected IWorkerModule workerModule;
		protected TaskGeneratorModule(ILogger Logger,ITaskModule TaskModule,IWorkerModule WorkerModule) : base(Logger)
		{
			this.taskModule = TaskModule;this.workerModule = WorkerModule;
		}

		protected Worker AssertWorkerIsIdle(int WorkerID,[CallerMemberName] string MethodName = null)
		{
			Worker worker;
			Task task;

			Log(LogLevels.Information, $"Checking if worker exists and is idle (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID) ).OrThrow<PIOInternalErrorException>($"Failed to get worker");
			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, MethodName);
			}

			task = Try(() => taskModule.GetLastTask(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get last worker task");
			if (task != null)
			{
				Log(LogLevels.Warning, $"Worker is not free (WorkerID={WorkerID})");
				throw new PIOInvalidOperationException($"Worker is not free (WorkerID={WorkerID})", null, ID, ModuleName, MethodName);
			}

			return worker;
		}

		protected virtual void OnTaskCreated(Task Task)
		{
			LogEnter();
			if (TaskCreated == null) return;
			Log(LogLevels.Information, $"Scheduling task (TaskID={Task.TaskID})");
			Try(() => TaskCreated(this, Task)).OrThrow<PIOInternalErrorException>("Failed to schedule task");
		}

	}
}
