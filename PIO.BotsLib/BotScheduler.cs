using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIO.BotsLib
{
	public class BotScheduler : AtModule<BotEvent>, IBotScheduler
	{
		private int retryDelay;
		public BotScheduler(ILogger Logger, int RetryDelay, ThreadPriority Priority = ThreadPriority.Normal, int StopTimeout = 5000) : base(Logger, Priority, StopTimeout)
		{
			this.retryDelay = RetryDelay;
		}

		public void Add(IBot Bot)
		{
			Add(DateTime.Now, new BotEvent(Bot));
		}
		
		protected override void OnTriggerEvent(BotEvent Event)
		{
			PIO.Models.Task task;
			bool result;

			LogEnter();

			#region check if worker is idle
			Log(LogLevels.Information, $"Checking if worker is idle (WorkerID={Event.Bot.WorkerID})");

			result = Try(() => Event.Bot.GetCurrentTask() ).OrAlert(out task,$"Failed to get worker tasks, waiting for {retryDelay}s before retry");
			if (!result)
			{
				Add(DateTime.Now.AddSeconds(retryDelay), Event);
				return;
			}
			else if (task != null)
			{
				Log(LogLevels.Warning, $"Worker is not idle (WorkerID={Event.Bot.WorkerID})");
				Add(task.ETA, Event);
				return;
			}
			#endregion

			#region enqueue new task
			Log(LogLevels.Information, $"Running new task (WorkerID={Event.Bot.WorkerID})");
			result = Try(() => Event.Bot.RunTask()).OrAlert(out task, $"Failed to run new task (WorkerID={Event.Bot.WorkerID})");
			if ((result) && (task != null))
			{
				Add(task.ETA, Event);
			}
			else
			{
				Log(LogLevels.Warning, $"No task returned, waiting for {retryDelay}s before retry");
				Add(DateTime.Now.AddSeconds(retryDelay), Event);
				return;
			}
			#endregion
		}






	}
}
