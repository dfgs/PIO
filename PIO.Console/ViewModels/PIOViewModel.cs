using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.Modules;
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
		protected ITranslationModule TranslationModule;

		public abstract string Header
		{
			get;
		}

		public PIOViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule)
		{
			this.PIOClient = PIOClient;this.BotsClient = BotsClient;this.TranslationModule = TranslationModule;
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

			model = await TryAsync(OnLoadModelAsync());
			if (model == null) return;
			await TryAsync(LoadAsync(model));

		}


	}
}
