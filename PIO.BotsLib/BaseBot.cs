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

		protected IPIOService Client
		{
			get;
			private set;
		}

        public BaseBot(ILogger Logger, IPIOService Client, int WorkerID, ThreadPriority Priority = ThreadPriority.Normal, int StopTimeout = 5000) :base(Logger,Priority,StopTimeout)
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
			if (delay.TotalMilliseconds>0) WaitHandles((int)(delay.TotalMilliseconds), QuitEvent);
		}

		protected override void ThreadLoop()
		{

			PIO.Models.Task task;
			bool result;

			LogEnter();

			#region if task is already running, wait...
			result = false; task = null;
			while ((!result) && (State == ModuleStates.Started))
			{
				Log(LogLevels.Information, $"Checking if worker is idle (WorkerID={WorkerID})");
				result = Try(() => Client.GetLastTask(WorkerID)).OrAlert(out task, "Failed to get worker tasks, waiting for 5s before retry");
				if (!result) WaitHandles(5000, QuitEvent);
				else if (task != null)
				{
					Log(LogLevels.Warning, $"Worker is not idle (WorkerID={WorkerID})");
					WaitETA(task.ETA);
				}
			}
			#endregion

			while (State == ModuleStates.Started)
			{
				Log(LogLevels.Information, $"Running new task (WorkerID={WorkerID})");
				task = OnRunTask();
				if (task == null)
				{
					Log(LogLevels.Warning, $"No task returned, waiting for 5s before retry");
					WaitHandles(5000, QuitEvent);
				}
				else
				{
					WaitETA(task.ETA);
				}
			} 

			



		}

	}
}
