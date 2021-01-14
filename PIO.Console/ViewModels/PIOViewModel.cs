using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace PIO.Console.ViewModels
{
	public abstract class PIOViewModel<T>:ViewModel<T>
	{
		protected PIOServiceClient PIOClient;
		protected BotsServiceClient BotsClient;


		public PIOViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient)
		{
			this.PIOClient = PIOClient;this.BotsClient = BotsClient;
		}

		protected virtual async Task OnRefreshAsync()
		{
			await LoadAsync();

		}
		public sealed override async Task RefreshAsync()
		{
			await OnRefreshAsync();
		}
		protected abstract Task<T> OnLoadModelAsync();

		public  async Task LoadAsync()
		{
			T model;

			try
			{
				model = await OnLoadModelAsync();
				await LoadAsync(model);
				ErrorMessage = null;
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}

		}


	}
}
