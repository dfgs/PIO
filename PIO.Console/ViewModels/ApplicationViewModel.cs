using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.ClientLib.TaskCallbackServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace PIO.Console.ViewModels
{
	public class ApplicationViewModel:PIOViewModel<int>
	{
		public static readonly DependencyProperty MapItemsProperty = DependencyProperty.Register("MapItems", typeof(MapItemsViewModel), typeof(ApplicationViewModel));
		public MapItemsViewModel MapItems
		{
			get { return (MapItemsViewModel)GetValue(MapItemsProperty); }
			set { SetValue(MapItemsProperty, value); }
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

		public ApplicationViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			MapItems = new MapItemsViewModel(PIOClient, BotsClient, 1);
			Workers = new WorkersViewModel(PIOClient,BotsClient,1);
			Factories = new FactoriesViewModel(PIOClient, BotsClient, 1);
	
		}

		protected override async Task<int> OnLoadModelAsync()
		{
			return await Task.FromResult(0);
		}

		protected override async Task OnLoadAsync(int Model)
		{
			await MapItems.LoadAsync();
			await Workers.LoadAsync();
			await Factories.LoadAsync();
		}


		public async Task OnTaskStarted(PIO.Models.Task Task)
		{
			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.CarryTo:
					await MapItems.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
				case Models.TaskTypeIDs.Produce:
					await MapItems.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
			}
		}
		public async Task OnTaskEnded(PIO.Models.Task Task)
		{
			await Workers.RefreshWorker(Task.WorkerID);
			switch (Task.TaskTypeID)
			{
				case Models.TaskTypeIDs.MoveTo:
					await MapItems.RefreshWorker(Task.WorkerID);
					break;
				case Models.TaskTypeIDs.CarryTo:
					await MapItems.RefreshWorker(Task.WorkerID);
					await MapItems.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
				case Models.TaskTypeIDs.Produce:
					await MapItems.RefreshFactory(Task.X.Value, Task.Y.Value);
					break;
			}
		}


	}
}
