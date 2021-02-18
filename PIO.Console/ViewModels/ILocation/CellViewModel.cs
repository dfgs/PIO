using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.Bots.Models;
using PIO.ClientLib.PIOServiceReference;
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
		public static readonly DependencyProperty CreateBuildFactoryOrderCommandProperty = DependencyProperty.Register("CreateBuildFactoryOrderCommand", typeof(ViewModelCommand), typeof(CellViewModel), new PropertyMetadata(null));
		public ViewModelCommand CreateBuildFactoryOrderCommand
		{
			get { return (ViewModelCommand)GetValue(CreateBuildFactoryOrderCommandProperty); }
			set { SetValue(CreateBuildFactoryOrderCommandProperty, value); }
		}
		public static readonly DependencyProperty CreateBuildFarmOrderCommandProperty = DependencyProperty.Register("CreateBuildFarmOrderCommand", typeof(ViewModelCommand), typeof(CellViewModel), new PropertyMetadata(null));
		public ViewModelCommand CreateBuildFarmOrderCommand
		{
			get { return (ViewModelCommand)GetValue(CreateBuildFarmOrderCommandProperty); }
			set { SetValue(CreateBuildFarmOrderCommandProperty, value); }
		}


		public int X
		{
			get { return Model.X; }
		}
		public int Y
		{
			get { return Model.Y; }
		}
		
		public string Description
		{
			get { return $"Cell {X},{Y}"; }
		}


		public static readonly DependencyProperty BuildFactoryOrdersProperty = DependencyProperty.Register("BuildFactoryOrders", typeof(BuildFactoryOrdersViewModel), typeof(CellViewModel));
		public BuildFactoryOrdersViewModel BuildFactoryOrders
		{
			get { return (BuildFactoryOrdersViewModel)GetValue(BuildFactoryOrdersProperty); }
			set { SetValue(BuildFactoryOrdersProperty, value); }
		}

		public static readonly DependencyProperty BuildFarmOrdersProperty = DependencyProperty.Register("BuildFarmOrders", typeof(BuildFarmOrdersViewModel), typeof(CellViewModel));
		public BuildFarmOrdersViewModel BuildFarmOrders
		{
			get { return (BuildFarmOrdersViewModel)GetValue(BuildFarmOrdersProperty); }
			set { SetValue(BuildFarmOrdersProperty, value); }
		}

		public CellViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			CreateBuildFactoryOrderCommand = new ViewModelCommand(CreateBuildFactoryOrderCommandCanExecute, CreateBuildFactoryOrderCommandExecute);
			CreateBuildFarmOrderCommand = new ViewModelCommand(CreateBuildFarmOrderCommandCanExecute, CreateBuildFarmOrderCommandExecute);
		}
		private async void MoveToCommandExecute(object obj)
		{
			await System.Threading.Tasks.Task.Yield();
		}//*/

		private bool CreateBuildFactoryOrderCommandCanExecute(object arg)
		{
			return (Model != null)&& (BuildFactoryOrders.Count == 0);
		}
		private async void CreateBuildFactoryOrderCommandExecute(object obj)
		{
			BuildFactoryOrder result;
			BuildFactoryOrderViewModel vm;

			result = await TryAsync(BotsClient.CreateBuildFactoryOrderAsync(Model.PlanetID, BuildingTypeIDs.Sawmill,Model.X,Model.Y));
			if (result == null) return;
			vm = new BuildFactoryOrderViewModel(PIOClient, BotsClient);
			await vm.LoadAsync(result);
			BuildFactoryOrders.Add(vm);
		}
		private bool CreateBuildFarmOrderCommandCanExecute(object arg)
		{
			return (Model != null) && (BuildFarmOrders.Count == 0);
		}
		private async void CreateBuildFarmOrderCommandExecute(object obj)
		{
			BuildFarmOrder result;
			BuildFarmOrderViewModel vm;

			result = await TryAsync(BotsClient.CreateBuildFarmOrderAsync(Model.PlanetID, BuildingTypeIDs.Forest, Model.X, Model.Y));
			if (result == null) return;
			vm = new BuildFarmOrderViewModel(PIOClient, BotsClient);
			await vm.LoadAsync(result);
			BuildFarmOrders.Add(vm);
		}
		protected override async System.Threading.Tasks.Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await BuildFactoryOrders.RefreshAsync();
			await BuildFarmOrders.RefreshAsync();
		}

		protected override async Task<Cell> OnLoadModelAsync()
		{
			return await PIOClient.GetCellAsync(((Cell)Model).CellID);
		}

		protected override async System.Threading.Tasks.Task OnLoadAsync(Cell Model)
		{
			BuildFactoryOrders = new BuildFactoryOrdersViewModel(PIOClient, BotsClient, Model.PlanetID,Model.X,Model.Y);
			await BuildFactoryOrders.LoadAsync();
			BuildFarmOrders = new BuildFarmOrdersViewModel(PIOClient, BotsClient, Model.PlanetID, Model.X, Model.Y);
			await BuildFarmOrders.LoadAsync();
		}


	}
}
