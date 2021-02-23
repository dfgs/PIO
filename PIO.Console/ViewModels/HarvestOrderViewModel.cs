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
	public class HarvestOrderViewModel : PIOViewModel<HarvestOrder>
	{

		

		public HarvestOrderViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, PhrasesViewModel PhrasesViewModel) : base(PIOClient, BotsClient,PhrasesViewModel)
		{

		}

		

		protected override async Task<HarvestOrder> OnLoadModelAsync()
		{
			return await BotsClient.GetHarvestOrderAsync(Model.HarvestOrderID);
		}

	}
}
