using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.Bots.Models;
using PIO.ClientLib.PIOServiceReference;
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

		

		public BuildOrderViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, PhrasesViewModel PhrasesViewModel) : base(PIOClient, BotsClient,PhrasesViewModel)
		{

		}

		

		protected override async Task<BuildOrder> OnLoadModelAsync()
		{
			return await BotsClient.GetBuildOrderAsync(Model.BuildOrderID);
		}

	}
}
