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
	public class ProduceOrdersViewModel : PIOViewModelCollection<ProduceOrderViewModel,ProduceOrder>
	{
		private int buildingID;
		public ProduceOrdersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int BuildingID) : base(PIOClient, BotsClient)
		{
			this.buildingID = BuildingID;
		}

		protected override ProduceOrderViewModel OnCreateItem(ProduceOrder Model)
		{
			return new ProduceOrderViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<ProduceOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetProduceOrdersForBuildingAsync(buildingID);
		}
		

	}
}
