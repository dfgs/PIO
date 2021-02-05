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
	public class BuildFactoryOrderViewModel : PIOViewModel<BuildFactoryOrder>
	{

		

		public BuildFactoryOrderViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{

		}

		

		protected override async Task<BuildFactoryOrder> OnLoadModelAsync()
		{
			return await BotsClient.GetBuildFactoryOrderAsync(Model.BuildFactoryOrderID);
		}

	}
}
