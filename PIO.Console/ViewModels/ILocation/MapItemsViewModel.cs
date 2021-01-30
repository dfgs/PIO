using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace PIO.Console.ViewModels
{
	public class MapItemsViewModel : ViewModelCollection<ILocationViewModel>
	{
		

		public MapItemsViewModel()
		{
		}

		
			
		public override async System.Threading.Tasks.Task RefreshAsync()
		{
			await System.Threading.Tasks.Task.Yield();
		}

	}
}
