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
	public class BuildingsViewModel : PIOViewModelCollection<BuildingViewModel,Building>, ILocationViewModelCollection
	{
		private int planetID;

		public BuildingsViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, PhrasesViewModel PhrasesViewModel, int PlanetID) : base(PIOClient, BotsClient,PhrasesViewModel)
		{
			this.planetID = PlanetID;
		}

		protected override BuildingViewModel OnCreateItem(Building Model)
		{
			return new BuildingViewModel(PIOClient, BotsClient, PhrasesViewModel);
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
