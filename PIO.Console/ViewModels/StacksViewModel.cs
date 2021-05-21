
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
	public class StacksViewModel : PIOViewModelCollection<StackViewModel,Stack>
	{
		public override string Header => TranslationModule.Translate("Stacks");

		private int buildingID;
		public StacksViewModel(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule, int BuildingID) : base(PIOClient, BotsClient, TranslationModule)
		{
			this.buildingID = BuildingID;
		}

		protected override StackViewModel OnCreateItem(Stack Model)
		{
			return new StackViewModel(PIOClient, BotsClient,TranslationModule);
		}

		protected override async Task<IEnumerable<Stack>> OnLoadModelAsync()
		{
			return await PIOClient.GetStacksAsync(buildingID);
		}
		

	}
}
