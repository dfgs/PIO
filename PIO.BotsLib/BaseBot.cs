using LogLib;
using ModuleLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.WebServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIO.BotsLib
{
	public abstract class BaseBot : ThreadModule, IBot
    {
		public int WorkerID
		{
			get;
			private set;
		}

		protected PIOServiceClient Client
		{
			get;
			private set;
		}

        public BaseBot(ILogger Logger, PIOServiceClient Client, int WorkerID, ThreadPriority Priority = ThreadPriority.Normal, int StopTimeout = 5000) :base(Logger,Priority,StopTimeout)
		{
			this.Client = Client;
			this.WorkerID = WorkerID;
		}

		protected abstract PIO.Models.Task OnRunTask();

		private void WaitETA(DateTime DateTime)
		{
			TimeSpan delay;
			LogEnter();

			delay = DateTime - DateTime.Now;
			Log(LogLevels.Information, $"Waiting for task to finish");
			if (delay.TotalMilliseconds>0) WaitHandles((int)delay.TotalMilliseconds, QuitEvent);
		}

		protected override void ThreadLoop()
		{

			PIO.Models.Task[] tasks;
			PIO.Models.Task task;
			bool result;

			LogEnter();


			#region check is worker is IDLE
			do
			{
				result = false; tasks = null;
				while ((!result) && (State == ModuleStates.Started))
				{
					Log(LogLevels.Information, $"Checking if worker is idle (WorkerID={WorkerID})");
					result = Try(() => Client.GetTasks(WorkerID)).OrAlert(out tasks, "Failed to get worker tasks, waiting for 5s before retry");
					if (!result) WaitHandles(5000, QuitEvent);
				}
				if ((tasks != null) && (tasks.Length > 0)) WaitETA(tasks[0].ETA);
			} while ((tasks.Length > 0) && (State == ModuleStates.Started));
			#endregion

			while (State == ModuleStates.Started)
			{
				Log(LogLevels.Information, $"Running new task (WorkerID={WorkerID})");
				task = OnRunTask();
				if (task==null)
				{
					Log(LogLevels.Warning, $"No task returned, waiting for 5s before retry");
					WaitHandles(5000, QuitEvent);
				}
				else WaitETA(task.ETA);
			}



		}

	}
}
