using PIO.Bots.ClientLib;
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
	public class BuildOrdersViewModel : PIOViewModelCollection<BuildOrderViewModel,BuildOrder>
	{
		private int planetID;

		public override string Header => TranslationModule.Translate("BuildOrders");

		public BuildOrdersViewModel(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule, int PlanetID) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.planetID = PlanetID;
		}

		protected override BuildOrderViewModel OnCreateItem(BuildOrder Model)
		{
			return new BuildOrderViewModel(PIOClient, BotsClient, TranslationModule);
		}

		protected override async Task<IEnumerable<BuildOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetBuildOrdersAsync(planetID);
		}
		

	}
}
