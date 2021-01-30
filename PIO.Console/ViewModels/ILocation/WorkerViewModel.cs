using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class WorkerViewModel : PIOViewModel<Worker>, ILocationViewModel
	{
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
			get { return $"Worker #{Model.WorkerID}"; }
		}

		public static readonly DependencyProperty TasksProperty = DependencyProperty.Register("Tasks", typeof(TasksViewModel), typeof(WorkerViewModel));
		public TasksViewModel Tasks
		{
			get { return (TasksViewModel)GetValue(TasksProperty); }
			set { SetValue(TasksProperty, value); }
		}


		public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Task", typeof(TaskViewModel), typeof(WorkerViewModel));
		public TaskViewModel Task
		{
			get { return (TaskViewModel)GetValue(TaskProperty); }
			set { SetValue(TaskProperty, value); }
		}

		public WorkerViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{

		}

		protected override async Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await Tasks.RefreshAsync();
		}

		protected override async Task<Worker> OnLoadModelAsync()
		{
			return await PIOClient.GetWorkerAsync(Model.WorkerID);
		}
		protected override async Task OnLoadAsync(Worker Model)
		{
			Tasks = new TasksViewModel(PIOClient, BotsClient,Model.WorkerID);
			await Tasks.LoadAsync();
			Task = Tasks.FirstOrDefault();
		}

	}
}
