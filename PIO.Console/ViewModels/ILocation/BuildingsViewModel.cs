using PIO.Bots.ClientLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.Modules;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class BuildingsViewModel : PIOViewModelCollection<BuildingViewModel,Building>, ILocationViewModelCollection
	{
		public override string Header => TranslationModule.Translate("Buildings");

		private int planetID;
		private ProduceOrdersViewModel produceOrdersViewModel;
		private HarvestOrdersViewModel harvestOrderViewModels;

		public BuildingsViewModel(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule, ProduceOrdersViewModel ProduceOrdersViewModel, HarvestOrdersViewModel HarvestOrderViewModels, int PlanetID) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.planetID = PlanetID;
			this.produceOrdersViewModel = ProduceOrdersViewModel;
			this.harvestOrderViewModels = HarvestOrderViewModels;
		}

		protected override BuildingViewModel OnCreateItem(Building Model)
		{
			return new BuildingViewModel(PIOClient, BotsClient, TranslationModule,produceOrdersViewModel,harvestOrderViewModels);
		}

		protected override async Task<IEnumerable<Building>> OnLoadModelAsync()
		{
			return await PIOClient.GetBuildingsAsync(planetID);
		}

		public async Task RefreshBuilding(int X,int Y)
		{
			BuildingViewModel vm;

			vm = this.FirstOrDefault(item => (item.Model?.X == X) && (item.Model?.Y==Y));
			if (vm != null) await vm.RefreshAsync();
		}

		public async Task RefreshBuilding(int BuildingID)
		{
			BuildingViewModel vm;

			vm = this.FirstOrDefault(item => item.Model?.BuildingID == BuildingID);
			if (vm != null) await vm.RefreshAsync();
		}
	}
}
