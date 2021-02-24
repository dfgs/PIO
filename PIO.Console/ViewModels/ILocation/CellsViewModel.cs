using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.Modules;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class CellsViewModel : PIOViewModelCollection<CellViewModel,Cell>, ILocationViewModelCollection
	{
		public override string Header => TranslationModule.Translate("Cells");

		private int planetID;

		private static int maxQueryWidth = 5;
		private static int maxQueryHeight = 5;

		private BuildOrdersViewModel buildOrdersViewModel;

		public CellsViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule, BuildOrdersViewModel BuildOrdersViewModel, int PlanetID) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.planetID = PlanetID;
			this.buildOrdersViewModel = BuildOrdersViewModel;
		}

		protected override CellViewModel OnCreateItem(Cell Model)
		{
			return new CellViewModel(PIOClient, BotsClient, TranslationModule, buildOrdersViewModel);
		}

		protected override async Task<IEnumerable<Cell>> OnLoadModelAsync()
		{
			Cell[] items;
			List<Cell> result;
			Planet planet;
			int x, y;

			planet = await PIOClient.GetPlanetAsync(planetID);

			result = new List<Cell>();
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
					
			return result;
			
		}
		

	}
}
