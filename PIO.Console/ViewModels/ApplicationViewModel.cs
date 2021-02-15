using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.ClientLib.TaskCallbackServiceReference;
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


		public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(MapItemsViewModel), typeof(ApplicationViewModel), new PropertyMetadata(null));
		public MapItemsViewModel SelectedItems
		{
			get { return (MapItemsViewModel)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}



		public static readonly DependencyProperty WorkersProperty = DependencyProperty.Register("Workers", typeof(WorkersViewModel), typeof(ApplicationViewModel));
		public WorkersViewModel Workers
		{
			get { return (WorkersViewModel)GetValue(WorkersProperty); }
			set { SetValue(WorkersProperty, value); }
		}


		public static readonly DependencyProperty FactoriesProperty = DependencyProperty.Register("Factories", typeof(FactoriesViewModel), typeof(ApplicationViewModel));
		public FactoriesViewModel Factories
		{
			get { return (FactoriesViewModel)GetValue(FactoriesProperty); }
			set { SetValue(FactoriesProperty, value); }
		}

		public static readonly DependencyProperty FarmsProperty = DependencyProperty.Register("Farms", typeof(FarmsViewModel), typeof(ApplicationViewModel));
		public FarmsViewModel Farms
		{
			get { return (FarmsViewModel)GetValue(FarmsProperty); }
			set { SetValue(FarmsProperty, value); }
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

		public ApplicationViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			Cells = new CellsViewModel(PIOClient, BotsClient, 1);
			Workers = new WorkersViewModel(PIOClient,BotsClient,1);
			Factories = new FactoriesViewModel(PIOClient, BotsClient, 1);
			Farms = new FarmsViewModel(PIOClient, BotsClient, 1);
			MapItems = new MapItemsViewModel() ;
			SelectedItems = new MapItemsViewModel();


		}

		protected override async Task<int> OnLoadModelAsync()
		{
			return await System.Threading.Tasks.Task.FromResult(0);
		}

		protected override async System.Threading.Tasks.Task OnLoadAsync(int Model)
		{
			await Cells.LoadAsync();
			await Workers.LoadAsync();
			await Factories.LoadAsync();
			await Farms.LoadAsync();

			await MapItems.LoadAsync(Cells.Union<ILocationViewModel>(Factories).Union(Farms).Union(Workers));
			

		}


		public async System.Threading.Tasks.Task OnTaskStarted(PIO.Models.Task Task)
		{
			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.Take:
					await Factories.RefreshFactory(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Produce:
					await Factories.RefreshFactory(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Build:
					await Factories.RefreshFactory(Task.X, Task.Y);
					break;
			}
		}
		public async System.Threading.Tasks.Task OnTaskEnded(PIO.Models.Task Task)
		{
			Factory factory;
			FactoryViewModel factoryViewModel;

			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.MoveTo:
					break;
				case Models.TaskTypeIDs.Store:
					await Factories.RefreshFactory(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Produce:
					await Factories.RefreshFactory(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.Build:
					await Factories.RefreshFactory(Task.X, Task.Y);
					break;
				case Models.TaskTypeIDs.CreateBuilding:
					try
					{
						factory = PIOClient.GetFactoryAtPos(1, Task.X, Task.Y);
						if (factory!=null)
						{
							factoryViewModel = new FactoryViewModel(PIOClient, BotsClient);
							await factoryViewModel.LoadAsync(factory);
							Factories.Add(factoryViewModel);
							MapItems.Insert(Cells.Count, factoryViewModel);
						}
					}
					catch(Exception ex)
					{
						ErrorMessage = ex.Message;
					}
					//await Factories.RefreshFactory(Task.X.Value, Task.Y.Value);
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
