using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.ServerLib.Modules.Executors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PIO.ServerLib.Modules
{
	public class TaskSchedulerModule : AtModule<ScheduledExecutor>,ITaskSchedulerModule
	{
		private readonly Dictionary<int, IExecutor> tasks;
		private readonly IScheduledTaskModule scheduledTaskModule;
		private readonly IMessageBrokerModule messageBrokerModule;
		private readonly IStackModule stackModule;

		public TaskSchedulerModule(ILogger Logger, IMessageBrokerModule MessageBrokerModule, IScheduledTaskModule ScheduledTaskModule, IFactoryModule FactoryModule,IMaterialModule MaterialModule ,IStackModule StackModule) : base(Logger)
		{
			IExecutor executor;

			this.scheduledTaskModule = ScheduledTaskModule;
			this.messageBrokerModule = MessageBrokerModule;

			tasks = new Dictionary<int, IExecutor>();

			executor = new NOPExecutor(Logger); tasks.Add(executor.TaskID, executor);
			executor = new CheckMaterialExecutor(Logger,FactoryModule,MaterialModule,StackModule); tasks.Add(executor.TaskID, executor);
		}

		protected override void OnTriggerEvent(ScheduledExecutor ScheduledExecutor)
		{
			int eventID;


			if (!Try<int>(() => ScheduledExecutor.Executor.Execute(ScheduledExecutor.FactoryID)).OrAlert(out eventID, "Unexpected error occured in executor")) return;
			// post event
			Try(() => scheduledTaskModule.DeleteScheduledTask(ScheduledExecutor.ScheduledTaskID)).OrThrow("Failed to delete scheduled task");
			Try(() => messageBrokerModule.Post(new Message(ScheduledExecutor.FactoryID, eventID))).OrAlert("Failed to post message to broker");
		}

		public void StartTask(int FactoryID, int TaskID, DateTime ETA)
		{
			IExecutor executor;
			ScheduledExecutor scheduledExecutor;
			int scheduledTaskID;

			LogEnter();

			scheduledTaskID=Try(() =>  scheduledTaskModule.CreateScheduledTask(FactoryID, TaskID, ETA)).OrThrow($"Failed to create scheduled task for factory {FactoryID}");

			if (!Try(() => tasks[TaskID]).OrWarn(out executor, $"Task {TaskID} is not registed")) return;

			scheduledExecutor = new ScheduledExecutor(executor, scheduledTaskID, FactoryID);
			Try(()=>this.Add(ETA, scheduledExecutor )).OrAlert($"Failed to schedule executor for factory {FactoryID}, task {TaskID}");

			//*/
		}





	}
}
