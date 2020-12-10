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
	public abstract class BaseBot : Module, IBot
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

        public BaseBot(ILogger Logger, IPIOService Client, int WorkerID) :base(Logger)
		{
			this.Client = Client;
			this.WorkerID = WorkerID;
		}

		public abstract PIO.Models.Task RunTask();

		public PIO.Models.Task GetCurrentTask()
		{
			PIO.Models.Task task;

			LogEnter();

			task = Try(() => Client.GetLastTask(WorkerID)).OrThrow<BotException>( "Failed to get worker tasks");
			return task;
		}

		/*private void WaitETA(DateTime DateTime)
		{
			TimeSpan delay;
			LogEnter();

			delay = DateTime - DateTime.Now;
			Log(LogLevels.Information, $"Waiting for task to finish");
			if (delay.TotalMilliseconds>0) WaitHandles((int)(delay.TotalMilliseconds), QuitEvent);
		}*/


	}
}
