using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.Bots.Models;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.Modules;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace PIO.Console.ViewModels
{
	public class CellViewModel : PIOViewModel<Cell>,ILocationViewModel
	{
		public static readonly DependencyProperty CreateBuildOrderCommandProperty = DependencyProperty.Register("CreateBuildOrderCommand", typeof(ViewModelCommand), typeof(CellViewModel), new PropertyMetadata(null));
		public ViewModelCommand CreateBuildOrderCommand
		{
			get { return (ViewModelCommand)GetValue(CreateBuildOrderCommandProperty); }
			set { SetValue(CreateBuildOrderCommandProperty, value); }
		}
		


		public int X
		{
			get { return Model.X; }
		}
		public int Y
		{
			get { return Model.Y; }
		}
		
		public override string Header
		{
			get { return $"{TranslationModule.Translate("Cell")} {X},{Y}"; }
		}


		private BuildOrdersViewModel buildOrdersViewModel;
		public IEnumerable<BuildOrderViewModel> BuildOrders
		{
			get { return buildOrdersViewModel.Where(item => (item.Model.X == Model.X)&& (item.Model.Y == Model.Y)); }
		}



		public CellViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule, BuildOrdersViewModel BuildOrdersViewModel) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.buildOrdersViewModel = BuildOrdersViewModel;
			CreateBuildOrderCommand = new ViewModelCommand(CreateBuildOrderCommandCanExecute, CreateBuildOrderCommandExecute);
		}
		private async void MoveToCommandExecute(object obj)
		{
			await System.Threading.Tasks.Task.Yield();
		}//*/

		private bool CreateBuildOrderCommandCanExecute(object arg)
		{
			return (Model != null);//&& (BuildFactoryOrders.Count == 0);
		}
		private async void CreateBuildOrderCommandExecute(object obj)
		{
			BuildOrder result;
			BuildOrderViewModel vm;

			result = await TryAsync(BotsClient.CreateBuildOrderAsync(Model.PlanetID, BuildingTypeIDs.Sawmill,Model.X,Model.Y));
			if (result == null) return;
			vm = new BuildOrderViewModel(PIOClient, BotsClient, TranslationModule);
			await vm.LoadAsync(result);
			buildOrdersViewModel.Add(vm);
			OnPropertyChanged("BuildOrders");
		}
		
		

		protected override async Task<Cell> OnLoadModelAsync()
		{
			return await PIOClient.GetCellAsync(((Cell)Model).CellID);
		}

		


	}
}
