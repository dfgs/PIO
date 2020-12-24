using LogLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIO.BotsLib.Basic
{
	public class ProducerBot : BaseBot
	{
		public int FactoryID
		{
			get;
			private set;
		}

		public ProducerBot(ILogger Logger, ClientLib.PIOServiceReference.IPIOService Client, int WorkerID,int FactoryID) : base(Logger, Client, WorkerID)
		{
			this.FactoryID = FactoryID;
			
		}

		public override Models.Task RunTask()
		{
			Factory factory;
			Worker worker;
			Models.Task task;
			bool result,hasEnoughResources;


			Log(LogLevels.Information, "Checking if worker is in factory");
			if (!Try(() => Client.GetWorker(WorkerID)).OrAlert(out worker, "Failed to get worker")) return null;
			
	
			if (!Try(() => Client.GetFactory(FactoryID)).OrAlert(out factory, "Failed to get factory")) return null;
			if (factory==null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exists (FactoryID={FactoryID})");
				return null;
			}
			

			if ((worker.X != factory.X)|| (worker.Y != factory.Y))
			{
				Log(LogLevels.Information, "Worker needs to move to factory");
				result = Try(() => Client.MoveTo(WorkerID, factory.X, factory.Y)).OrAlert(out task, $"Failed to run task MoveTo (WorkerID={WorkerID})");
				return task;
			}

			Log(LogLevels.Information, $"Checking if factory has enough resources (FactoryID={FactoryID})");
			result = Try(() => Client.HasEnoughResourcesToProduce(FactoryID)).OrAlert(out hasEnoughResources, $"Failed to check if factory has enough resources (FactoryID={FactoryID})");
			if (!hasEnoughResources)
			{
				Log(LogLevels.Warning, "Factory doesn't have enough resources to produce");
				return null;
			}

			Log(LogLevels.Information, $"Trying to run task Produce (WorkerID={WorkerID})");
			result = Try(() => Client.Produce(WorkerID)).OrAlert(out task, $"Failed to run task Produce (WorkerID={WorkerID})");
			
			return task;
		}



	}
}
