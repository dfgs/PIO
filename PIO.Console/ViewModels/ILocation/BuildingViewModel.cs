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
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class BuildingViewModel :PIOViewModel<Building>, ILocationViewModel
	{

		public static readonly DependencyProperty CreateProduceOrderCommandProperty = DependencyProperty.Register("CreateProduceOrderCommand", typeof(ViewModelCommand), typeof(BuildingViewModel), new PropertyMetadata(null));
		public ViewModelCommand CreateProduceOrderCommand
		{
			get { return (ViewModelCommand)GetValue(CreateProduceOrderCommandProperty); }
			set { SetValue(CreateProduceOrderCommandProperty, value); }
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
			get { return $"Building #{Model.BuildingID}"; }
		}

		public static readonly DependencyProperty StacksProperty = DependencyProperty.Register("Stacks", typeof(StacksViewModel), typeof(BuildingViewModel));
		public StacksViewModel Stacks
		{
			get { return (StacksViewModel)GetValue(StacksProperty); }
			set { SetValue(StacksProperty, value); }
		}
		
		public static readonly DependencyProperty ProduceOrdersProperty = DependencyProperty.Register("ProduceOrders", typeof(ProduceOrdersViewModel), typeof(BuildingViewModel));
		public ProduceOrdersViewModel ProduceOrders
		{
			get { return (ProduceOrdersViewModel)GetValue(ProduceOrdersProperty); }
			set { SetValue(ProduceOrdersProperty, value); }
		}


		public BuildingViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			CreateProduceOrderCommand = new ViewModelCommand(CreateProduceOrderCommandCanExecute, CreateProduceOrderCommandExecute);
		}

		private bool CreateProduceOrderCommandCanExecute(object arg)
		{
			return (Model != null) && (Model.RemainingBuildSteps==0) && (ProduceOrders.Count==0);
		}
		private async void CreateProduceOrderCommandExecute(object obj)
		{
			ProduceOrder result;
			ProduceOrderViewModel vm;

			result=await TryAsync(BotsClient.CreateProduceOrderAsync(Model.PlanetID, Model.BuildingID));
			if (result == null) return;
			vm = new ProduceOrderViewModel(PIOClient, BotsClient);
			await vm.LoadAsync(result);
			ProduceOrders.Add(vm);
		}

		

		protected override async Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await Stacks.RefreshAsync();
			await ProduceOrders.RefreshAsync();
		}

		protected override async Task<Building> OnLoadModelAsync()
		{
			return await PIOClient.GetBuildingAsync(Model.BuildingID);
		}
		protected override async Task OnLoadAsync(Building Model)
		{
			Stacks = new StacksViewModel(PIOClient, BotsClient, Model.BuildingID);
			await Stacks.LoadAsync();
			ProduceOrders = new ProduceOrdersViewModel(PIOClient, BotsClient, Model.BuildingID);
			await ProduceOrders.LoadAsync();
		}
		
	
	}
}
