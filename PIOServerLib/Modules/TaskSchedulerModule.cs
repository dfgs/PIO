using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIOServerLib.Modules.Executors;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PIOServerLib.Modules
{
	public class TaskSchedulerModule : AtModule<ScheduledExecutor>,ITaskSchedulerModule
	{
		private Dictionary<int, IExecutor> tasks;
		private IScheduledTaskModule scheduledTaskModule;

		public TaskSchedulerModule(ILogger Logger, IScheduledTaskModule ScheduledTaskModule) : base(Logger)
		{
			IExecutor executor;

			this.scheduledTaskModule = ScheduledTaskModule;
			tasks = new Dictionary<int, IExecutor>();

			executor = new NOPExecutor(Logger); tasks.Add(executor.TaskID, executor);
		}

		protected override void OnTriggerEvent(ScheduledExecutor ScheduledExecutor)
		{
			int eventID;

			eventID=ScheduledExecutor.Executor.Execute(ScheduledExecutor.FactoryID);
			
		}

		public void StartTask(int FactoryID, int TaskID, DateTime ETA)
		{
			IExecutor executor;
			ScheduledExecutor scheduledExecutor;

			LogEnter();

			Try(() => { scheduledTaskModule.CreateScheduledTask(FactoryID, TaskID, ETA); }).OrThrow($"Failed to create scheduled task for FactoryID {FactoryID}");

			if (!Try(() => tasks[TaskID]).OrWarn(out executor, $"TaskID {TaskID} is not registed")) return;

			scheduledExecutor = new ScheduledExecutor() { Executor = executor, FactoryID = FactoryID };
			Try(()=>this.Add(ETA, scheduledExecutor )).OrAlert($"Failed to schedule executor for FactoryID {FactoryID}, TaskID {TaskID}");

			//*/
		}





	}
}
