using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Console.ViewModels
{
	public class CellViewModel : PIOViewModel<Cell>
	{
		public CellViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
		}


		protected override async Task<Cell> OnLoadModelAsync()
		{
			return await PIOClient.GetCellAsync(((Cell)Model).CellID);
		}
	}
}
