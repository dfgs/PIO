using LogLib;
using ModuleLib;
using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IPIOService = PIO.ClientLib.PIOServiceReference.IPIOService;

namespace PIO.Bots.ServerLib.Modules
{
	public class BotSchedulerModule : AtModule<int>, IBotSchedulerModule
	{
		private int retryDelay;
		private IPIOService client;
		private IOrderManagerModule orderManager;
		private IBotModule botModule;

		public BotSchedulerModule(ILogger Logger, IPIOService Client,IBotModule BotModule, IOrderManagerModule OrderManager, int RetryDelay, ThreadPriority Priority = ThreadPriority.Normal, int StopTimeout = 5000) : base(Logger, Priority, StopTimeout)
		{
			this.client = Client;
			this.botModule = BotModule;
			this.orderManager = OrderManager;
			this.retryDelay = RetryDelay;
		}

		/*public void Add(int WorkerID)
		{
			Add(DateTime.Now, WorkerID);
		}*/

		private void LoadWorkers()
		{
			Bot[] items;

			LogEnter();

			Log(LogLevels.Information, $"Loading existing bots");
			if (!Try(() => botModule.GetBots()).OrAlert(out items, "Failed to load Bots"))
			{
				//Add(DateTime.Now.AddSeconds(retryDelay), -1);
				return;
			}

			foreach (Bot item in items)
			{
				Add(DateTime.Now, item.BotID);
			}
		}

		public Bot CreateBot(int WorkerID)
		{
			Bot item;

			LogEnter();

			Log(LogLevels.Information, $"Trying to create bot");
			item=Try(() => botModule.CreateBot(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create Bot");
			Add(DateTime.Now, item.BotID);

			return item;
		}
		public void DeleteBot(int BotID)
		{
			Bot item;

			LogEnter();

			Log(LogLevels.Information, $"Trying to get bot");
			item = Try(() => botModule.GetBot(BotID)).OrThrow<PIOInternalErrorException>("Failed to get Bot");
			if (item == null)
			{
				Log(LogLevels.Warning, $"Bot doesn't exist (BotID={BotID})");
				throw new PIONotFoundException($"Bot doesn't exist (BotID={BotID})", null, ID, ModuleName, "DeleteBot");
			}

			Remove((int id) => id == BotID );

			#region clear bot assignment
			Log(LogLevels.Information, $"Clearing bot assignment (BotID={BotID})");
			Try(() => orderManager.UnassignAll(BotID)).OrThrow<PIOInternalErrorException>($"Failed to clear bot assignment");
			
			Log(LogLevels.Information, $"Deleting bot (BotID={BotID})");
			Try(() => botModule.DeleteBot(BotID)).OrThrow<PIOInternalErrorException>($"Failed to delete bot");

			#endregion

		}

		protected override void OnStarting()
		{
			LogEnter();
			LoadWorkers();
		}


		protected override void OnTriggerEvent(int BotID)
		{
			PIO.Models.Task task;
			bool result;
			Bot bot;

			LogEnter();

			/*if (BotID==-1)
			{
				LoadBots();
				return;
			}*/

			#region clear bot assignment
			Log(LogLevels.Information, $"Clearing bot assignment (BotID={BotID})");
			result = Try(() => orderManager.UnassignAll(BotID)).OrAlert($"Failed to clear bot assignment, waiting for {retryDelay}s before retry");
			if (!result)
			{
				Add(DateTime.Now.AddSeconds(retryDelay), BotID);
				return;
			}
			#endregion

			#region get bot
			Log(LogLevels.Information, $"Trying to get bot (BotID={BotID})");
			result = Try(() => botModule.GetBot(BotID)).OrAlert(out bot, $"Failed to get bot, waiting for {retryDelay}s before retry");
			if (!result)
			{
				Add(DateTime.Now.AddSeconds(retryDelay), BotID);
				return;
			}
			if (bot==null)
			{
				Log(LogLevels.Warning, $"Bot doesn't exist (BotID={BotID})");
				return;
			}
			#endregion

			#region check if worker is idle

			Log(LogLevels.Information, $"Checking if worker is idle (WorkerID={bot.WorkerID})");

			result=Try(() => client.GetLastTask(BotID) ).OrAlert(out task, $"Failed to get worker tasks, waiting for {retryDelay}s before retry");
			if (!result)
			{
				Add(DateTime.Now.AddSeconds(retryDelay), BotID);
				return;
			}
			else if (task != null)
			{
				Log(LogLevels.Warning, $"Worker is not idle (WorkerID={bot.WorkerID})");
				Add(task.ETA, BotID);
				return;
			}
			#endregion
			
			#region enqueue new task
			Log(LogLevels.Information, $"Running new task (WorkerID={bot.WorkerID})");
			result = Try(() => orderManager.CreateTask(bot.BotID, bot.WorkerID)).OrAlert(out task, $"Failed to run new task (WorkerID={bot.WorkerID})");
			if ((result) && (task != null))
			{
				Add(task.ETA, BotID);
			}
			else
			{
				Log(LogLevels.Warning, $"No task returned, waiting for {retryDelay}s before retry");
				Add(DateTime.Now.AddSeconds(retryDelay), BotID);
				return;
			}
			#endregion
		}

		
	}
}
