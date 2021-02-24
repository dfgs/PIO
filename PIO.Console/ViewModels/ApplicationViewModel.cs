using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.ClientLib.TaskCallbackServiceReference;
using PIO.Console.Modules;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace PIO.Console.ViewModels
{
	public class ApplicationViewModel:PIOViewModel<int>
	{
		public override string Header => "PIO.Console";

		private static int planetID = 1;


		public static readonly DependencyProperty LoadDurationProperty = DependencyProperty.Register("LoadDuration", typeof(TimeSpan), typeof(ApplicationViewModel));
		public TimeSpan LoadDuration
		{
			get { return (TimeSpan)GetValue(LoadDurationProperty); }
			set { SetValue(LoadDurationProperty, value); }
		}




		public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(MapItemsViewModel), typeof(ApplicationViewModel), new PropertyMetadata(null));
		public MapItemsViewModel SelectedItems
		{
			get { return (MapItemsViewModel)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		public static readonly DependencyProperty ProduceOrdersProperty = DependencyProperty.Register("ProduceOrders", typeof(ProduceOrdersViewModel), typeof(ApplicationViewModel));
		public ProduceOrdersViewModel ProduceOrders
		{
			get { return (ProduceOrdersViewModel)GetValue(ProduceOrdersProperty); }
			set { SetValue(ProduceOrdersProperty, value); }
		}

		public static readonly DependencyProperty HarvestOrdersProperty = DependencyProperty.Register("HarvestOrders", typeof(HarvestOrdersViewModel), typeof(ApplicationViewModel));
		public HarvestOrdersViewModel HarvestOrders
		{
			get { return (HarvestOrdersViewModel)GetValue(HarvestOrdersProperty); }
			set { SetValue(HarvestOrdersProperty, value); }
		}

		public static readonly DependencyProperty BuildOrdersProperty = DependencyProperty.Register("BuildOrders", typeof(BuildOrdersViewModel), typeof(ApplicationViewModel));
		public BuildOrdersViewModel BuildOrders
		{
			get { return (BuildOrdersViewModel)GetValue(BuildOrdersProperty); }
			set { SetValue(BuildOrdersProperty, value); }
		}


		public static readonly DependencyProperty WorkersProperty = DependencyProperty.Register("Workers", typeof(WorkersViewModel), typeof(ApplicationViewModel));
		public WorkersViewModel Workers
		{
			get { return (WorkersViewModel)GetValue(WorkersProperty); }
			set { SetValue(WorkersProperty, value); }
		}


		public static readonly DependencyProperty BuildingsProperty = DependencyProperty.Register("Buildings", typeof(BuildingsViewModel), typeof(ApplicationViewModel));
		public BuildingsViewModel Buildings
		{
			get { return (BuildingsViewModel)GetValue(BuildingsProperty); }
			set { SetValue(BuildingsProperty, value); }
		}

		

		public static readonly DependencyProperty CellsProperty = DependencyProperty.Register("Cells", typeof(CellsViewModel), typeof(ApplicationViewModel));
		public CellsViewModel Cells
		{
			get { return (CellsViewModel)GetValue(CellsProperty); }
			set { SetValue(CellsProperty, value); }
		}

		public static readonly DependencyProperty MapItemsProperty = DependencyProperty.Register("MapItems", typeof(MapItemsViewModel), typeof(ApplicationViewModel));
		public MapItemsViewModel MapItems
		{
			get { return (MapItemsViewModel)GetValue(MapItemsProperty); }
			set { SetValue(MapItemsProperty, value); }
		}

		public ApplicationViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient, ITranslationModule TranslationModule) : base(PIOClient, BotsClient,TranslationModule)
		{
			ProduceOrders = new ProduceOrdersViewModel(PIOClient, BotsClient, TranslationModule, planetID);
			HarvestOrders = new HarvestOrdersViewModel(PIOClient, BotsClient, TranslationModule, planetID);
			BuildOrders = new BuildOrdersViewModel(PIOClient, BotsClient, TranslationModule, planetID);

			Cells = new CellsViewModel(PIOClient, BotsClient, TranslationModule, BuildOrders, planetID);
			Workers = new WorkersViewModel(PIOClient,BotsClient, TranslationModule, planetID);
			Buildings = new BuildingsViewModel(PIOClient, BotsClient, TranslationModule, ProduceOrders,HarvestOrders, planetID);
			MapItems = new MapItemsViewModel() ;
			SelectedItems = new MapItemsViewModel();


		}

		protected override async Task<int> OnLoadModelAsync()
		{
			return await System.Threading.Tasks.Task.FromResult(0);
		}

		protected override async System.Threading.Tasks.Task OnLoadAsync(int Model)
		{
			DateTime start, stop;

			start = DateTime.Now;
			await ProduceOrders.LoadAsync();
			await HarvestOrders.LoadAsync();
			await BuildOrders.LoadAsync();

			await Cells.LoadAsync();
			await Workers.LoadAsync();
			await Buildings.LoadAsync();

			await MapItems.LoadAsync(Cells.Union<ILocationViewModel>(Buildings).Union(Workers));
			stop = DateTime.Now;

			LoadDuration = stop - start;

		}


		public async System.Threading.Tasks.Task OnTaskStarted(PIO.Models.Task Task)
		{
			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.Take:
					await Buildings.RefreshBuilding(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Produce:
					await Buildings.RefreshBuilding(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Build:
					await Buildings.RefreshBuilding(Task.X, Task.Y);
					break;
			}
		}
		public async System.Threading.Tasks.Task OnTaskEnded(PIO.Models.Task Task)
		{
			Building building;
			BuildingViewModel factoryViewModel;

			

			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.MoveTo:
					break;
				case Models.TaskTypeIDs.Store:
					await Buildings.RefreshBuilding(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Produce:
					await Buildings.RefreshBuilding(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Harvest:
					await Buildings.RefreshBuilding(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Build:
					await Buildings.RefreshBuilding(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.CreateBuilding:
					try
					{
						building = PIOClient.GetBuildingAtPos(1, Task.X, Task.Y);
						if (building!=null)
						{
							factoryViewModel = new BuildingViewModel(PIOClient, BotsClient, TranslationModule, ProduceOrders,HarvestOrders);
							await factoryViewModel.LoadAsync(building);
							Buildings.Add(factoryViewModel);
							MapItems.Insert(Cells.Count, factoryViewModel);
						}
					}
					catch(Exception ex)
					{
						ErrorMessage = ex.Message;
					}
					

					//await Buildings.RefreshBuilding(Task.X.Value, Task.Y.Value);
					break;
			}
		}

		public async System.Threading.Tasks.Task SelectAtAsync(int X, int Y)
		{
			foreach (CellViewModel viewModel in Cells)
			{
				viewModel.IsSelected = (viewModel.X == X) && (viewModel.Y == Y);
			}

			foreach (WorkerViewModel viewModel in Workers)
			{
				viewModel.IsSelected = (viewModel.X == X) && (viewModel.Y == Y);
			}

			await SelectedItems.LoadAsync(MapItems.Where(item => (item.X == X) && (item.Y == Y))) ;
			SelectedItems.SelectedItem = SelectedItems.FirstOrDefault();
		}
		public async System.Threading.Tasks.Task RunCommandAsync(int X, int Y)
		{

			foreach (WorkerViewModel worker in Workers.Where(item => (item.Bot == null) && (item.IsSelected)))
			{
				await TryAsync(PIOClient.MoveToAsync(worker.Model.WorkerID,X,Y) );
			}

		}
			

	}
}
