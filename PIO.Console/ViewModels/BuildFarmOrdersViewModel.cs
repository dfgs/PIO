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
	public class BuildFarmOrdersViewModel : PIOViewModelCollection<BuildFarmOrderViewModel,BuildFarmOrder>
	{
		private int planetID;
		private int X;
		private int Y;

		public BuildFarmOrdersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int PlanetID,int X,int Y) : base(PIOClient, BotsClient)
		{
			this.planetID = PlanetID;this.X = X;this.Y = Y;
		}

		protected override BuildFarmOrderViewModel OnCreateItem(BuildFarmOrder Model)
		{
			return new BuildFarmOrderViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<BuildFarmOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetBuildFarmOrdersAtPositionAsync(planetID, X, Y);
		}
		

	}
}
