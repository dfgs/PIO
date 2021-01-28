using PIO.Bots.ClientLib.BotsServiceReference;
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
	public class MapItemsViewModel : PIOViewModelCollection<MapItemViewModel,ILocation>
	{
		private int planetID;

		private static int maxQueryWidth = 5;
		private static int maxQueryHeight = 5;

		public MapItemsViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int PlanetID) : base(PIOClient, BotsClient)
		{
			this.planetID = PlanetID;
		}

		protected override MapItemViewModel OnCreateItem(ILocation Model)
		{
			if (Model is Cell) return new MapCellViewModel(PIOClient, BotsClient);
			if (Model is Factory) return new MapFactoryViewModel(PIOClient, BotsClient);
			if (Model is Worker) return new MapWorkerViewModel(PIOClient, BotsClient);
			throw new InvalidProgramException("Invalid map item type");
		}

		protected override async Task<IEnumerable<ILocation>> OnLoadModelAsync()
		{
			List<ILocation> result;

			ILocation[] items;
			Planet planet;
			int x, y;

			planet = await PIOClient.GetPlanetAsync(planetID);

			result = new List<ILocation>();
			x = 0;
			while(x<planet.Width)
			{
				y = 0;
				while(y<=planet.Height)
				{
					items=await PIOClient.GetCellsAsync(planetID, x, y, maxQueryWidth, maxQueryHeight);
					result.AddRange(items);
					y += maxQueryHeight;
				}
				x += maxQueryWidth;
			}

			items = await PIOClient.GetFactoriesAsync(planetID);
			result.AddRange(items);

			items = await PIOClient.GetWorkersAsync(planetID);
			result.AddRange(items);
					
			return result;
			
		}
		public async Task RefreshWorker(int WorkerID)
		{
			MapItemViewModel vm;

			vm = this.OfType<MapWorkerViewModel>().FirstOrDefault(item =>(((Worker)item.Model)?.WorkerID==WorkerID));
			if (vm != null) await vm.RefreshAsync();
		}
		public async Task RefreshFactory(int X, int Y)
		{
			MapFactoryViewModel vm;

			vm = this.OfType<MapFactoryViewModel>().FirstOrDefault(item => (item.Model?.X == X) && (item.Model?.Y == Y));
			if (vm != null) await vm.RefreshAsync();
		}

		/*public async Task RefreshFactory(int FactoryID)
		{
			MapFactoryViewModel vm;

			vm = this.OfType<MapFactoryViewModel>().FirstOrDefault(item => item.Model?.FactoryID == FactoryID);
			if (vm != null) await vm.RefreshAsync();
		}*/

	}
}
