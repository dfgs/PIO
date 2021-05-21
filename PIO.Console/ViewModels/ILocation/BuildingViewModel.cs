using PIO.Bots.ClientLib;
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
		public static readonly DependencyProperty CreateHarvestOrderCommandProperty = DependencyProperty.Register("CreateHarvestOrderCommand", typeof(ViewModelCommand), typeof(BuildingViewModel), new PropertyMetadata(null));
		public ViewModelCommand CreateHarvestOrderCommand
		{
			get { return (ViewModelCommand)GetValue(CreateHarvestOrderCommandProperty); }
			set { SetValue(CreateHarvestOrderCommandProperty, value); }
		}





		public string BuildingType
		{
			get { return TranslationModule.Translate(Model.BuildingTypeID.ToString()); }
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
			get { return $"{TranslationModule.Translate("Building")} #{Model.BuildingID}"; }
		}

		public static readonly DependencyProperty StacksProperty = DependencyProperty.Register("Stacks", typeof(StacksViewModel), typeof(BuildingViewModel));
		public StacksViewModel Stacks
		{
			get { return (StacksViewModel)GetValue(StacksProperty); }
			set { SetValue(StacksProperty, value); }
		}

		private ProduceOrdersViewModel produceOrdersViewModel;
		public IEnumerable<ProduceOrderViewModel> ProduceOrders
		{
			get { return produceOrdersViewModel.Where(item=>item.Model.BuildingID==Model.BuildingID); }
		}
		private HarvestOrdersViewModel harvestOrderViewModels;
		public IEnumerable<HarvestOrderViewModel> HarvestOrders
		{
			get { return harvestOrderViewModels.Where(item => item.Model.BuildingID == Model.BuildingID); }
		}


		public BuildingViewModel(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule, ProduceOrdersViewModel ProduceOrdersViewModel,HarvestOrdersViewModel HarvestOrderViewModels) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.produceOrdersViewModel = ProduceOrdersViewModel;
			this.harvestOrderViewModels = HarvestOrderViewModels;

			CreateProduceOrderCommand = new ViewModelCommand(CreateProduceOrderCommandCanExecute, CreateProduceOrderCommandExecute);
			CreateHarvestOrderCommand = new ViewModelCommand(CreateHarvestOrderCommandCanExecute, CreateHarvestOrderCommandExecute);


		}

		private bool CreateProduceOrderCommandCanExecute(object arg)
		{
			return (Model != null) && (Model.RemainingBuildSteps == 0);//&& (ProduceOrders.Count==0);
		}
		private async void CreateProduceOrderCommandExecute(object obj)
		{
			ProduceOrder result;
			ProduceOrderViewModel vm;

			result=await TryAsync(BotsClient.CreateProduceOrderAsync(Model.PlanetID, Model.BuildingID));
			if (result == null) return;
			vm = new ProduceOrderViewModel(PIOClient, BotsClient,TranslationModule);
			await vm.LoadAsync(result);
			produceOrdersViewModel.Add(vm);
			OnPropertyChanged("ProduceOrders");
		}

		private bool CreateHarvestOrderCommandCanExecute(object arg)
		{
			return (Model != null) && (Model.RemainingBuildSteps == 0);// && (HarvestOrders.Count == 0);
		}
		private async void CreateHarvestOrderCommandExecute(object obj)
		{
			HarvestOrder result;
			HarvestOrderViewModel vm;

			result = await TryAsync(BotsClient.CreateHarvestOrderAsync(Model.PlanetID, Model.BuildingID));
			if (result == null) return;
			vm = new HarvestOrderViewModel(PIOClient, BotsClient,TranslationModule);
			await vm.LoadAsync(result);
			harvestOrderViewModels.Add(vm);
			OnPropertyChanged("HarvestOrders");
		}


		protected override async Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await Stacks.RefreshAsync();
		}

		protected override async Task<Building> OnLoadModelAsync()
		{
			return await PIOClient.GetBuildingAsync(Model.BuildingID);
		}
		protected override async Task OnLoadAsync(Building Model)
		{
			Stacks = new StacksViewModel(PIOClient, BotsClient, TranslationModule, Model.BuildingID);
			await Stacks.LoadAsync();
			
		}


	}
}
