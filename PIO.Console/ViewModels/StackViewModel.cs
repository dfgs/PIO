using PIO.Bots.ClientLib.BotsServiceReference;
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
	public class StackViewModel : PIOViewModel<Stack>
	{
		public override string Header => TranslationModule.Translate("Stack");

		public string ResourceType
		{
			get { return TranslationModule.Translate(Model.ResourceTypeID.ToString()); }
		}

		public StackViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule) : base(PIOClient, BotsClient,TranslationModule)
		{

		}

		

		protected override async Task<Stack> OnLoadModelAsync()
		{
			return await PIOClient.GetStackAsync(Model.StackID);
		}

	}
}
