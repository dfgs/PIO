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
	public class ProduceOrdersViewModel : PIOViewModelCollection<ProduceOrderViewModel,ProduceOrder>
	{
		public override string Header => TranslationModule.Translate("ProduceOrders");

		private int planetID;
		public ProduceOrdersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule, int PlanetID) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.planetID = PlanetID;
		}

		protected override ProduceOrderViewModel OnCreateItem(ProduceOrder Model)
		{
			return new ProduceOrderViewModel(PIOClient, BotsClient,TranslationModule);
		}

		protected override async Task<IEnumerable<ProduceOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetProduceOrdersAsync(planetID);
		}
		

	}
}
