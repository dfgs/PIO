using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.Bots.Models;
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
	public class HarvestOrdersViewModel : PIOViewModelCollection<HarvestOrderViewModel,HarvestOrder>
	{
		private int buildingID;
		public HarvestOrdersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, PhrasesViewModel PhrasesViewModel, int BuildingID) : base(PIOClient, BotsClient,PhrasesViewModel)
		{
			this.buildingID = BuildingID;
		}

		protected override HarvestOrderViewModel OnCreateItem(HarvestOrder Model)
		{
			return new HarvestOrderViewModel(PIOClient, BotsClient,PhrasesViewModel);
		}

		protected override async Task<IEnumerable<HarvestOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetHarvestOrdersForBuildingAsync(buildingID);
		}
		

	}
}
