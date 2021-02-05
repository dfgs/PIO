using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.Bots.Models;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class BuildFactoryOrdersViewModel : PIOViewModelCollection<BuildFactoryOrderViewModel,BuildFactoryOrder>
	{
		private int planetID;
		private int X;
		private int Y;

		public BuildFactoryOrdersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int PlanetID,int X,int Y) : base(PIOClient, BotsClient)
		{
			this.planetID = PlanetID;this.X = X;this.Y = Y;
		}

		protected override BuildFactoryOrderViewModel OnCreateItem(BuildFactoryOrder Model)
		{
			return new BuildFactoryOrderViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<BuildFactoryOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetBuildFactoryOrdersAtPositionAsync(planetID, X, Y);
		}
		

	}
}
