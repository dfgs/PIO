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
	public class FarmViewModel :PIOViewModel<Farm>, ILocationViewModel
	{

		/*public static readonly DependencyProperty CreateProduceOrderCommandProperty = DependencyProperty.Register("CreateProduceOrderCommand", typeof(ViewModelCommand), typeof(FarmViewModel), new PropertyMetadata(null));
		public ViewModelCommand CreateProduceOrderCommand
		{
			get { return (ViewModelCommand)GetValue(CreateProduceOrderCommandProperty); }
			set { SetValue(CreateProduceOrderCommandProperty, value); }
		}*/


			



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
			get { return $"Farm #{Model.FarmID}"; }
		}

		public static readonly DependencyProperty StacksProperty = DependencyProperty.Register("Stacks", typeof(StacksViewModel), typeof(FarmViewModel));
		public StacksViewModel Stacks
		{
			get { return (StacksViewModel)GetValue(StacksProperty); }
			set { SetValue(StacksProperty, value); }
		}
		
		/*public static readonly DependencyProperty ProduceOrdersProperty = DependencyProperty.Register("ProduceOrders", typeof(ProduceOrdersViewModel), typeof(FarmViewModel));
		public ProduceOrdersViewModel ProduceOrders
		{
			get { return (ProduceOrdersViewModel)GetValue(ProduceOrdersProperty); }
			set { SetValue(ProduceOrdersProperty, value); }
		}*/


		public FarmViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			//CreateProduceOrderCommand = new ViewModelCommand(CreateProduceOrderCommandCanExecute, CreateProduceOrderCommandExecute);
		}

		/*private bool CreateProduceOrderCommandCanExecute(object arg)
		{
			return (Model != null) && (Model.RemainingBuildSteps==0) && (ProduceOrders.Count==0);
		}
		private async void CreateProduceOrderCommandExecute(object obj)
		{
			ProduceOrder result;
			ProduceOrderViewModel vm;

			result=await TryAsync(BotsClient.CreateProduceOrderAsync(Model.PlanetID, Model.FarmID));
			if (result == null) return;
			vm = new ProduceOrderViewModel(PIOClient, BotsClient);
			await vm.LoadAsync(result);
			ProduceOrders.Add(vm);
		}//*/

		

		protected override async Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await Stacks.RefreshAsync();
			//await ProduceOrders.RefreshAsync();
		}

		protected override async Task<Farm> OnLoadModelAsync()
		{
			return await PIOClient.GetFarmAsync(Model.FarmID);
		}
		protected override async Task OnLoadAsync(Farm Model)
		{
			Stacks = new StacksViewModel(PIOClient, BotsClient, Model.BuildingID);
			await Stacks.LoadAsync();
			//ProduceOrders = new ProduceOrdersViewModel(PIOClient, BotsClient, Model.FarmID);
			//await ProduceOrders.LoadAsync();
		}
		
	
	}
}
