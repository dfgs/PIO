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
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class BuildOrderViewModel : PIOViewModel<BuildOrder>
	{

		public override string Header => TranslationModule.Translate("BuildOrder");


		public BuildOrderViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule) : base(PIOClient, BotsClient,TranslationModule)
		{

		}

		

		protected override async Task<BuildOrder> OnLoadModelAsync()
		{
			return await BotsClient.GetBuildOrderAsync(Model.BuildOrderID);
		}

	}
}
