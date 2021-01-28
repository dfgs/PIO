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
	public abstract class PIOViewModelCollection<T,ModelT>:ViewModelCollection<T>
		where T:PIOViewModel<ModelT>
	{
		protected PIOServiceClient PIOClient;
		protected BotsServiceClient BotsClient;


		public PIOViewModelCollection(PIOServiceClient PIOClient, BotsServiceClient BotsClient)
		{
			this.PIOClient = PIOClient;this.BotsClient = BotsClient;
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

			items = new List<T>();


			try
			{
				models = await OnLoadModelAsync();
				
				foreach (ModelT model in models)
				{
					vm = OnCreateItem(model);
					await vm.LoadAsync(model);
					items.Add(vm);
				}

				await LoadAsync(items);
				ErrorMessage = null;
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}

		}

	}
}
