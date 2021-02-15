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
	public class FarmsViewModel : PIOViewModelCollection<FarmViewModel,Farm>, ILocationViewModelCollection
	{
		private int planetID;

		public FarmsViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int PlanetID) : base(PIOClient, BotsClient)
		{
			this.planetID = PlanetID;
		}

		protected override FarmViewModel OnCreateItem(Farm Model)
		{
			return new FarmViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<Farm>> OnLoadModelAsync()
		{
			return await PIOClient.GetFarmsAsync(planetID);
		}

		public async Task RefreshFarm(int X,int Y)
		{
			FarmViewModel vm;

			vm = this.FirstOrDefault(item => (item.Model?.X == X) && (item.Model?.Y==Y));
			if (vm != null) await vm.RefreshAsync();
		}

		public async Task RefreshFarm(int FarmID)
		{
			FarmViewModel vm;

			vm = this.FirstOrDefault(item => item.Model?.FarmID == FarmID);
			if (vm != null) await vm.RefreshAsync();
		}
	}
}
