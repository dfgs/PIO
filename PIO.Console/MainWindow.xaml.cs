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

namespace PIO.Console
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private PIOServiceClient pioClient;
		private BotsServiceClient botsClient;
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
				pioClient = new PIOServiceClient();
				pioClient.Open();
				botsClient = new BotsServiceClient();
				botsClient.Open();

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

	}
}
