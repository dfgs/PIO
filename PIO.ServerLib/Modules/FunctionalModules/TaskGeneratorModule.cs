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

		protected TaskGeneratorModule(ILogger Logger,ITaskModule TaskModule) : base(Logger)
		{
			this.taskModule = TaskModule;
		}

		protected DateTime GetLastETA(int WorkerID)
		{
			Task task;

			LogEnter();
			
			Log(LogLevels.Information, $"Getting last task (WorkerID={WorkerID})");
			task=Try(() => taskModule.GetLastTask(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get last task");
			if (task == null) return DateTime.Now;
			return task.ETA;
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
