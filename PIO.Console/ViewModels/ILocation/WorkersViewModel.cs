using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class WorkersViewModel : PIOViewModelCollection<WorkerViewModel,Worker>, ILocationViewModelCollection
	{
		private int planetID;


		


		public WorkersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int PlanetID) : base(PIOClient, BotsClient)
		{
			this.planetID = PlanetID;
		}


		

		protected override WorkerViewModel OnCreateItem(Worker Model)
		{
			return new WorkerViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<Worker>> OnLoadModelAsync()
		{
			return await PIOClient.GetWorkersAsync(planetID);
		}
		
		public async Task RefreshWorker(int WorkerID)
		{
			WorkerViewModel vm;

			vm = this.FirstOrDefault(item => item.Model?.WorkerID == WorkerID);
			if (vm != null) await vm.RefreshAsync();
		}

	}
}
