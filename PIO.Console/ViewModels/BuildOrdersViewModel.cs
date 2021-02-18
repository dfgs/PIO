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
	public class BuildOrdersViewModel : PIOViewModelCollection<BuildOrderViewModel,BuildOrder>
	{
		private int planetID;
		private int X;
		private int Y;

		public BuildOrdersViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int PlanetID,int X,int Y) : base(PIOClient, BotsClient)
		{
			this.planetID = PlanetID;this.X = X;this.Y = Y;
		}

		protected override BuildOrderViewModel OnCreateItem(BuildOrder Model)
		{
			return new BuildOrderViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<BuildOrder>> OnLoadModelAsync()
		{
			return await BotsClient.GetBuildOrdersAtPositionAsync(planetID, X, Y);
		}
		

	}
}
