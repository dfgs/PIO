using LogLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.WebServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIO.BotsLib.Basic
{
	public class IdleBot : BaseBot
	{
		public int IdleDuration
		{
			get;
			private set;
		}

		public IdleBot(ILogger Logger, IPIOService Client, int WorkerID, int IdleDuration, ThreadPriority Priority = ThreadPriority.Normal, int StopTimeout = 5000) : base(Logger, Client, WorkerID, Priority, StopTimeout)
		{
			this.IdleDuration = IdleDuration;
		}

		protected override Models.Task OnRunTask()
		{
			Models.Task task;
			bool result;

		
			Log(LogLevels.Information, $"Trying to run task Idle (WorkerID={WorkerID})");
			result=Try(()=>Client.Idle(WorkerID, IdleDuration)).OrAlert(out task,$"Failed to run task Idle (WorkerID={WorkerID})");

			return task;
		}

	}
}
