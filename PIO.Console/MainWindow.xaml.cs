using System;
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
using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.Console.ViewModels;
using System.ServiceModel;
using PIO.ClientLib.TaskCallbackServiceReference;
using PIO.Models;

namespace PIO.Console
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window,ITaskCallbackServiceCallback
	{
		private PIOServiceClient pioClient;
		private BotsServiceClient botsClient;
		private TaskCallbackServiceClient taskCallbackClient;

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
				botsClient = new BotsServiceClient(new BasicHttpBinding(), new EndpointAddress($@"http://127.0.0.1:8734/PIO/Bots/Service"));
				botsClient.Open();
				taskCallbackClient = new TaskCallbackServiceClient(new InstanceContext(this),  new WSDualHttpBinding(), new EndpointAddress($@"http://localhost:8735/PIO/TaskCallback/Service"));
				taskCallbackClient.Open();
				taskCallbackClient.Subscribe();
				isConnected = true;
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}

			ApplicationViewModel = new ApplicationViewModel(pioClient, botsClient);
			DataContext = ApplicationViewModel;

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

			try
			{
				botsClient?.Close();
			}
			catch { }

			DataContext = null;
			ApplicationViewModel = null;
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


	}
}
