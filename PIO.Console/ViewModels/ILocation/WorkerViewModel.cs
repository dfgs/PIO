﻿using PIO.Bots.ClientLib;
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

		public override string Header
		{
			get { return $"{TranslationModule.Translate("Worker")} #{Model.WorkerID}"; }
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

		public static readonly DependencyProperty ProduceCommandProperty = DependencyProperty.Register("ProduceCommand", typeof(ViewModelCommand), typeof(WorkerViewModel), new PropertyMetadata(null));
		public ViewModelCommand ProduceCommand
		{
			get { return (ViewModelCommand)GetValue(ProduceCommandProperty); }
			set { SetValue(ProduceCommandProperty, value); }
		}
		public static readonly DependencyProperty HarvestCommandProperty = DependencyProperty.Register("HarvestCommand", typeof(ViewModelCommand), typeof(WorkerViewModel), new PropertyMetadata(null));
		public ViewModelCommand HarvestCommand
		{
			get { return (ViewModelCommand)GetValue(HarvestCommandProperty); }
			set { SetValue(HarvestCommandProperty, value); }
		}



		public WorkerViewModel(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule) : base(PIOClient, BotsClient,TranslationModule)
		{
			CreateBotCommand = new ViewModelCommand(CreateBotCommandCanExecute, CreateBotCommandExecute);
			DeleteBotCommand = new ViewModelCommand(DeleteBotCommandCanExecute, DeleteBotCommandExecute);

			ProduceCommand = new ViewModelCommand(ProduceCommandCanExecute, ProduceCommandExecute);
			HarvestCommand = new ViewModelCommand(HarvestCommandCanExecute, HarvestCommandExecute);

		}

		private bool CreateBotCommandCanExecute(object arg)
		{
			return (Model != null)&& (Bot == null);
		}
		private async void CreateBotCommandExecute(object obj)
		{
			Bot result;

			result = await TryAsync(BotsClient.CreateBotAsync(Model.WorkerID));
			if (result == null) return;
			Bot = new BotViewModel(PIOClient, BotsClient, TranslationModule);
			await Bot.LoadAsync(result);
		}
		private bool DeleteBotCommandCanExecute(object arg)
		{
			return (Model != null) && (Bot != null);
		}
		private async void DeleteBotCommandExecute(object obj)
		{

			await TryAsync(BotsClient.DeleteBotAsync(Bot.Model.BotID));
			Bot = null;
		}



		private bool ProduceCommandCanExecute(object arg)
		{
			return (Model != null) && (Bot == null);
		}
		private async void ProduceCommandExecute(object obj)
		{
			PIO.Models.Task result;

			result = await TryAsync(PIOClient.ProduceAsync(Model.WorkerID));
			if (result == null) return;
		}
		private bool HarvestCommandCanExecute(object arg)
		{
			return (Model != null) && (Bot == null);
		}
		private async void HarvestCommandExecute(object obj)
		{
			PIO.Models.Task result;

			result = await TryAsync(PIOClient.HarvestAsync(Model.WorkerID));
			if (result == null) return;
		}










		protected override async Task OnRefreshAsync()
		{
			Bot bot;

			await base.OnRefreshAsync();
			await Tasks.RefreshAsync();
			
			bot = await BotsClient.GetBotForWorkerAsync(Model.WorkerID);
			if (bot == null)
			{
				Bot = null;
			}
			else
			{
				Bot = new BotViewModel(PIOClient, BotsClient, TranslationModule);
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

			Tasks = new TasksViewModel(PIOClient, BotsClient,TranslationModule, Model.WorkerID);
			await Tasks.LoadAsync();
			Task = Tasks.FirstOrDefault();

			bot = await BotsClient.GetBotForWorkerAsync(Model.WorkerID);
			if (bot == null)
			{
				Bot = null;
			}
			else
			{
				Bot = new BotViewModel(PIOClient, BotsClient, TranslationModule);
				await Bot.LoadAsync(bot);
			}
		}

	}
}
