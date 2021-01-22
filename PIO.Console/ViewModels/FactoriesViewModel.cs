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
	public class FactoriesViewModel : PIOViewModelCollection<FactoryViewModel,Factory>
	{
		private int planetID;

		public FactoriesViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int PlanetID) : base(PIOClient, BotsClient)
		{
			this.planetID = PlanetID;
		}

		protected override FactoryViewModel OnCreateItem()
		{
			return new FactoryViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<Factory>> OnLoadModelAsync()
		{
			return await PIOClient.GetFactoriesAsync(planetID);
		}

		public async Task RefreshFactory(int X,int Y)
		{
			FactoryViewModel vm;

			vm = this.FirstOrDefault(item => (item.Model?.X == X) && (item.Model?.Y==Y));
			if (vm != null) await vm.RefreshAsync();
		}

		public async Task RefreshFactory(int FactoryID)
		{
			FactoryViewModel vm;

			vm = this.FirstOrDefault(item => item.Model?.FactoryID == FactoryID);
			if (vm != null) await vm.RefreshAsync();
		}
	}
}
