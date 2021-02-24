using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.Modules;
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

		public override string Header => TranslationModule.Translate("Workers");



		public WorkersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule, int PlanetID) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.planetID = PlanetID;
		}


		

		protected override WorkerViewModel OnCreateItem(Worker Model)
		{
			return new WorkerViewModel(PIOClient, BotsClient,TranslationModule);
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
