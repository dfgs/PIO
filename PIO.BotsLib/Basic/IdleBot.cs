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

		public IdleBot(ILogger Logger, IPIOService Client, int WorkerID, int IdleDuration) : base(Logger, Client, WorkerID)
		{
			this.IdleDuration = IdleDuration;
		}

		public override Models.Task RunTask()
		{
			Models.Task task;
		
			Log(LogLevels.Information, $"Trying to run task Idle (WorkerID={WorkerID})");
			task=Try(()=>Client.Idle(WorkerID, IdleDuration)).OrThrow<BotException>($"Failed to run task Idle (WorkerID={WorkerID})");

			return task;
		}

	}
}
