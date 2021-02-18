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
		
		public string Description
		{
			get { return $"Cell {X},{Y}"; }
		}


		public static readonly DependencyProperty BuildFactoryOrdersProperty = DependencyProperty.Register("BuildFactoryOrders", typeof(BuildOrdersViewModel), typeof(CellViewModel));
		public BuildOrdersViewModel BuildFactoryOrders
		{
			get { return (BuildOrdersViewModel)GetValue(BuildFactoryOrdersProperty); }
			set { SetValue(BuildFactoryOrdersProperty, value); }
		}

		

		public CellViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			CreateBuildOrderCommand = new ViewModelCommand(CreateBuildOrderCommandCanExecute, CreateBuildOrderCommandExecute);
		}
		private async void MoveToCommandExecute(object obj)
		{
			await System.Threading.Tasks.Task.Yield();
		}//*/

		private bool CreateBuildOrderCommandCanExecute(object arg)
		{
			return (Model != null)&& (BuildFactoryOrders.Count == 0);
		}
		private async void CreateBuildOrderCommandExecute(object obj)
		{
			BuildOrder result;
			BuildOrderViewModel vm;

			result = await TryAsync(BotsClient.CreateBuildOrderAsync(Model.PlanetID, BuildingTypeIDs.Sawmill,Model.X,Model.Y));
			if (result == null) return;
			vm = new BuildOrderViewModel(PIOClient, BotsClient);
			await vm.LoadAsync(result);
			BuildFactoryOrders.Add(vm);
		}
		
		protected override async System.Threading.Tasks.Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await BuildFactoryOrders.RefreshAsync();
		}

		protected override async Task<Cell> OnLoadModelAsync()
		{
			return await PIOClient.GetCellAsync(((Cell)Model).CellID);
		}

		protected override async System.Threading.Tasks.Task OnLoadAsync(Cell Model)
		{
			BuildFactoryOrders = new BuildOrdersViewModel(PIOClient, BotsClient, Model.PlanetID,Model.X,Model.Y);
			await BuildFactoryOrders.LoadAsync();
		}


	}
}
