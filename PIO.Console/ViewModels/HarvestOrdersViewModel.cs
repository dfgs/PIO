using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.Bots.Models;
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
	public class HarvestOrdersViewModel : PIOViewModelCollection<HarvestOrderViewModel,HarvestOrder>
	{
		public override string Header => TranslationModule.Translate("HarvestOrders");

		private int planetID;
		public HarvestOrdersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule, int PlanetID) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.planetID = PlanetID;
		}

		protected override HarvestOrderViewModel OnCreateItem(HarvestOrder Model)
		{
			return new HarvestOrderViewModel(PIOClient, BotsClient,TranslationModule);
		}

		protected override async Task<IEnumerable<HarvestOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetHarvestOrdersAsync(planetID);
		}
		

	}
}
