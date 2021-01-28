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
	public class StacksViewModel : PIOViewModelCollection<StackViewModel,Stack>
	{
		private int buildingID;
		public StacksViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int BuildingID) : base(PIOClient, BotsClient)
		{
			this.buildingID = BuildingID;
		}

		protected override StackViewModel OnCreateItem(Stack Model)
		{
			return new StackViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<Stack>> OnLoadModelAsync()
		{
			return await PIOClient.GetStacksAsync(buildingID);
		}
		

	}
}
