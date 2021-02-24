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

namespace PIO.Console.ViewModels
{
	public class BotViewModel : PIOViewModel<Bot>
	{

		public override string Header => TranslationModule.Translate("Bot");

		public BotViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule) : base(PIOClient, BotsClient,TranslationModule)
		{
		}

		

		protected override async Task<Bot> OnLoadModelAsync()
		{
			throw new NotImplementedException();
		}

	}
}
