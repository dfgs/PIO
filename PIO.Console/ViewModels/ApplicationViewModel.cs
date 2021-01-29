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

		public static readonly DependencyProperty CellsProperty = DependencyProperty.Register("Cells", typeof(CellsViewModel), typeof(ApplicationViewModel));
		public CellsViewModel Cells
		{
			get { return (CellsViewModel)GetValue(CellsProperty); }
			set { SetValue(CellsProperty, value); }
		}

		public static readonly DependencyProperty MapItemsProperty = DependencyProperty.Register("MapItems", typeof(ObservableCollection<object>), typeof(ApplicationViewModel));
		public ObservableCollection<object> MapItems
		{
			get { return (ObservableCollection<object>)GetValue(MapItemsProperty); }
			set { SetValue(MapItemsProperty, value); }
		}

		public ApplicationViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			Cells = new CellsViewModel(PIOClient, BotsClient, 1);
			Workers = new WorkersViewModel(PIOClient,BotsClient,1);
			Factories = new FactoriesViewModel(PIOClient, BotsClient, 1);
			
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

			MapItems = new ObservableCollection<object>(Cells.Cast<object>().Union(Factories).Union(Workers) );
			
		}


		public async System.Threading.Tasks.Task OnTaskStarted(PIO.Models.Task Task)
		{
			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.CarryTo:
					await Factories.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
				case Models.TaskTypeIDs.Produce:
					await Factories.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
			}
		}
		public async System.Threading.Tasks.Task OnTaskEnded(PIO.Models.Task Task)
		{
			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.MoveTo:
					break;
				case Models.TaskTypeIDs.CarryTo:
					await Factories.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
				case Models.TaskTypeIDs.Produce:
					await Factories.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
			}
		}


	}
}
