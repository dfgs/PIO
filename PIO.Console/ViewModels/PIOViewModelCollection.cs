using PIO.Bots.ClientLib;
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
	public abstract class PIOViewModelCollection<T,ModelT>:ViewModelCollection<T>
		where T:PIOViewModel<ModelT>
	{
		protected PIOServiceClient PIOClient;
		protected BotsRESTClient BotsClient;
		protected ITranslationModule TranslationModule;

		public abstract string Header
		{
			get;
		}

		public PIOViewModelCollection(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule)
		{
			this.PIOClient = PIOClient;this.BotsClient = BotsClient;this.TranslationModule = TranslationModule;
		}

		
		public override async Task RefreshAsync()
		{
			await LoadAsync();
		}

		protected abstract Task<IEnumerable<ModelT>> OnLoadModelAsync();
		protected abstract T OnCreateItem(ModelT Model);

		public async Task LoadAsync()
		{
			IEnumerable<ModelT> models;
			T vm;
			List<T> items;
			bool result;

			items = new List<T>();

			models = await TryAsync(OnLoadModelAsync());
			if (models == null) return;

			foreach (ModelT model in models)
			{
				vm = OnCreateItem(model);
				result=await TryAsync(vm.LoadAsync(model));
				if (result) items.Add(vm);
			}

			await TryAsync(LoadAsync(items));
		}


	}
}
