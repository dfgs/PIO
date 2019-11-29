using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.Models;

namespace PIO.ServerLib.Modules
{
	public class TaskSchedulerModule : AtModule<Task>,ITaskSchedulerModule
	{
		private readonly ITaskModule taskModule;

		public TaskSchedulerModule(ILogger Logger, ITaskModule TaskModule) : base(Logger)
		{

			this.taskModule = TaskModule;
			
		}

		public bool Initialize()
		{
			Task[] tasks;

			Log(LogLevels.Information, "Retrieving tasks");
			if (!Try<Task[]>( ()=>taskModule.GetTasks().ToArray() ).OrAlert(out tasks,"Failed to retrieve tasks")) return false;

			foreach(Task task in tasks)
			{
				Add(task.ETA, task);
			}

			return true;
		}

		protected override void OnTriggerEvent(Task Task)
		{
			int t = 0;
			/*
			int eventID;


			if (!Try<int>(() => ScheduledExecutor.Executor.Execute(ScheduledExecutor.FactoryID)).OrAlert(out eventID, "Unexpected error occured in executor")) return;
			// post event
			Try(() => taskModule.DeleteScheduledTask(ScheduledExecutor.ScheduledTaskID)).OrThrow("Failed to delete scheduled task");
			Try(() => messageBrokerModule.Post(new Message(ScheduledExecutor.FactoryID, eventID))).OrAlert("Failed to post message to broker");
			*/
		}

		





	}
}
