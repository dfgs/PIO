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

		public static readonly DependencyProperty BotProperty = DependencyProperty.Register("Bot", typeof(BotViewModel), typeof(WorkerViewModel));
		public BotViewModel Bot
		{
			get { return (BotViewModel)GetValue(BotProperty); }
			set { SetValue(BotProperty, value); }
		}



		public static readonly DependencyProperty CreateBotCommandProperty = DependencyProperty.Register("CreateBotCommand", typeof(ViewModelCommand), typeof(WorkerViewModel), new PropertyMetadata(null));
		public ViewModelCommand CreateBotCommand
		{
			get { return (ViewModelCommand)GetValue(CreateBotCommandProperty); }
			set { SetValue(CreateBotCommandProperty, value); }
		}
		public static readonly DependencyProperty DeleteBotCommandProperty = DependencyProperty.Register("DeleteBotCommand", typeof(ViewModelCommand), typeof(WorkerViewModel), new PropertyMetadata(null));
		public ViewModelCommand DeleteBotCommand
		{
			get { return (ViewModelCommand)GetValue(DeleteBotCommandProperty); }
			set { SetValue(DeleteBotCommandProperty, value); }
		}


		public WorkerViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			CreateBotCommand = new ViewModelCommand(CreateBotCommandCanExecute, CreateBotCommandExecute);

		}

		private bool CreateBotCommandCanExecute(object arg)
		{
			return (Model != null) && (Bot == null);
		}
		private async void CreateBotCommandExecute(object obj)
		{
			Bot result;

			result = await TryAsync(BotsClient.CreateBotAsync(Model.WorkerID));
			if (result == null) return;
			Bot = new BotViewModel(PIOClient, BotsClient);
			await Bot.LoadAsync(result);
		}

		protected override async Task OnRefreshAsync()
		{
			Bot bot;

			await base.OnRefreshAsync();
			await Tasks.RefreshAsync();
			
			bot = BotsClient.GetBotForWorker(Model.WorkerID);
			if (bot == null)
			{
				Bot = null;
			}
			else
			{
				Bot = new BotViewModel(PIOClient, BotsClient);
				await Bot.LoadAsync(bot);
			}
		}

		protected override async Task<Worker> OnLoadModelAsync()
		{
			return await PIOClient.GetWorkerAsync(Model.WorkerID);
		}
		protected override async Task OnLoadAsync(Worker Model)
		{
			Bot bot;

			Tasks = new TasksViewModel(PIOClient, BotsClient,Model.WorkerID);
			await Tasks.LoadAsync();
			Task = Tasks.FirstOrDefault();

			bot = BotsClient.GetBotForWorker(Model.WorkerID);
			if (bot == null)
			{
				Bot = null;
			}
			else
			{
				Bot = new BotViewModel(PIOClient, BotsClient);
				await Bot.LoadAsync(bot);
			}
		}

	}
}
