﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.ViewModels;
using System.ServiceModel;
using PIO.ClientLib.TaskCallbackServiceReference;
using PIO.Models;
using PIO.Console.Views;
using PIO.Console.Modules;
using LogLib;
using PIO.Bots.ClientLib;
using RESTLib.Client;

namespace PIO.Console
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window,ITaskCallbackServiceCallback
	{
		private PIOServiceClient pioClient;
		private BotsRESTClient botsClient;
		private TaskCallbackServiceClient taskCallbackClient;

		private TranslationModule translationModule;

		private bool isConnected;


		public static readonly DependencyProperty ApplicationViewModelProperty = DependencyProperty.Register("ApplicationViewModel", typeof(ApplicationViewModel), typeof(MainWindow));
		public ApplicationViewModel ApplicationViewModel
		{
			get { return (ApplicationViewModel)GetValue(ApplicationViewModelProperty); }
			set { SetValue(ApplicationViewModelProperty, value); }
		}

		public MainWindow()
		{
			InitializeComponent();

		}
		private void ShowError(Exception ex)
		{
			MessageBox.Show(ex.Message, "Error");
		}



		private void OpenCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !isConnected; e.Handled = true;
		}

		private async void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				pioClient = new PIOServiceClient(new BasicHttpBinding(), new EndpointAddress($@"http://127.0.0.1:8733/PIO/Service/"));
				pioClient.Open();
				botsClient = new BotsRESTClient($@"http://127.0.0.1:8734", new HttpConnector(), new ResponseDeserializer());
				taskCallbackClient = new TaskCallbackServiceClient(new InstanceContext(this),  new WSDualHttpBinding(), new EndpointAddress($@"http://localhost:8735/PIO/TaskCallback/Service"));
				taskCallbackClient.Open();
				taskCallbackClient.Subscribe();
				isConnected = true;
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}

			translationModule = new TranslationModule(NullLogger.Instance, pioClient);


			ApplicationViewModel = new ApplicationViewModel(pioClient, botsClient, translationModule);
			DataContext = ApplicationViewModel;

			await translationModule.LoadAsync("FR");
			await ApplicationViewModel.LoadAsync();
		}
		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = isConnected; e.Handled = true;
		}

		private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				taskCallbackClient?.Unsubscribe();
				taskCallbackClient?.Close();
			}
			catch { }
			try
			{
				pioClient?.Close();
			}
			catch { }

			/*try
			{
				botsClient?.Close();
			}
			catch { }*/

			DataContext = null;
			ApplicationViewModel = null;
			translationModule = null;
			pioClient = null;botsClient = null;
	
			isConnected = false;
			
		}

		public async void OnTaskStarted(Models.Task Task)
		{
			await ApplicationViewModel.OnTaskStarted(Task);
		}

		public async void OnTaskEnded(Models.Task Task)
		{
			await ApplicationViewModel.OnTaskEnded(Task); 
		}

		private async void MapView_MapLeftClicked(object sender, RoutedEventArgs e)
		{
			MapClickedRoutedEventArgs args;

			args = e as MapClickedRoutedEventArgs;
			await ApplicationViewModel.SelectAtAsync(args.X, args.Y);
		}

		private async void MapView_MapRightClicked(object sender, RoutedEventArgs e)
		{
			MapClickedRoutedEventArgs args;

			args = e as MapClickedRoutedEventArgs;
			await ApplicationViewModel.RunCommandAsync(args.X, args.Y);
		}


	}
}
