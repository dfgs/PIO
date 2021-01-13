using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class WorkersViewModel : PIOViewModelCollection<WorkerViewModel,Worker>
	{
		public WorkersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{

		}

		protected override WorkerViewModel OnCreateItem()
		{
			return new WorkerViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<Worker>> OnLoadModelAsync()
		{
			return await PIOClient.GetWorkersAsync(1);
		}
		

	}
}
