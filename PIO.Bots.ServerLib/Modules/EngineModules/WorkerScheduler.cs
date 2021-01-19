using LogLib;
using ModuleLib;
using PIO.Bots.Models.Modules;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IPIOService = PIO.ClientLib.PIOServiceReference.IPIOService;

namespace PIO.Bots.ServerLib.Modules
{
	public class WorkerScheduler : AtModule<int>, IWorkerScheduler
	{
		private int retryDelay;
		private IPIOService client;
		private IOrderManagerModule orderManager;

		public WorkerScheduler(ILogger Logger, IPIOService Client, IOrderManagerModule OrderManager, int RetryDelay, ThreadPriority Priority = ThreadPriority.Normal, int StopTimeout = 5000) : base(Logger, Priority, StopTimeout)
		{
			this.client = Client;this.orderManager = OrderManager;
			this.retryDelay = RetryDelay;
		}

		/*public void Add(int WorkerID)
		{
			Add(DateTime.Now, WorkerID);
		}*/

		private void LoadWorkers()
		{
			Worker[] items;

			LogEnter();

			Log(LogLevels.Information, $"Loading existing workers");
			if (!Try(() => client.GetAllWorkers()).OrAlert(out items, "Failed to load Workers"))
			{
				Add(DateTime.Now.AddSeconds(retryDelay), -1);
				return;
			}

			foreach (Worker item in items)
			{
				Add(DateTime.Now, item.WorkerID);
			}
		}
		protected override void OnStarting()
		{
			LogEnter();
			LoadWorkers();
		}


		protected override void OnTriggerEvent(int WorkerID)
		{
			PIO.Models.Task task;
			bool result;

			LogEnter();

			if (WorkerID==-1)
			{
				LoadWorkers();
				return;
			}

			#region clear worker assignment
			Log(LogLevels.Information, $"Clearing worker assignment (WorkerID={WorkerID})");
			result = Try(() => orderManager.UnassignAll(WorkerID)).OrAlert($"Failed to clear worker assignment (WorkerID={WorkerID})");
			if (!result)
			{
				Add(DateTime.Now.AddSeconds(retryDelay), WorkerID);
				return;
			}
			#endregion

			#region check if worker is idle
			Log(LogLevels.Information, $"Checking if worker is idle (WorkerID={WorkerID})");

			result=Try(() => client.GetLastTask(WorkerID) ).OrAlert(out task, $"Failed to get worker tasks, waiting for {retryDelay}s before retry");
			if (!result)
			{
				Add(DateTime.Now.AddSeconds(retryDelay), WorkerID);
				return;
			}
			else if (task != null)
			{
				Log(LogLevels.Warning, $"Worker is not idle (WorkerID={WorkerID})");
				Add(task.ETA, WorkerID);
				return;
			}
			#endregion
			
			#region enqueue new task
			Log(LogLevels.Information, $"Running new task (WorkerID={WorkerID})");
			result=Try(() => orderManager.CreateTask(WorkerID) ).OrAlert(out task, $"Failed to run new task (WorkerID={WorkerID})");
			if ((result) && (task != null))
			{
				Add(task.ETA, WorkerID);
			}
			else
			{
				Log(LogLevels.Warning, $"No task returned, waiting for {retryDelay}s before retry");
				Add(DateTime.Now.AddSeconds(retryDelay), WorkerID);
				return;
			}
			#endregion
		}






	}
}
