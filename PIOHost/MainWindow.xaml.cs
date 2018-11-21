using LogLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using NetORMLib.VersionControl;
using PIOClientLib;
using PIOServerLib;
using PIOViewModelLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace PIOHost
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ILogger logger;
	
		private IPIOClient client;
		private PIOServer server;

		private AppViewModel appViewModel;

		public MainWindow()
		{
			logger = new ConsoleLogger(new DefaultLogFormatter());

			// run server as if it was an external app
			server = new PIOServer(logger);
			server.Start();

			client = new HostedPIOClient(logger, server);


			InitializeComponent();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			server.Stop();
			base.OnClosing(e);
		}

		private void ShowError(Exception ex)
		{
			MessageBox.Show(this, ex.Message);
		}

		private void ConnectCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;e.CanExecute = !client.IsConnected;
		}

		private void ConnectCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			try
			{
				client.Connect();
				appViewModel = new AppViewModel(logger);
				appViewModel.Load(client);
				DataContext = appViewModel;
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		private void DisconnectCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = client.IsConnected;
		}

		private void DisconnectCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			try
			{
				client.Disconnect();
				DataContext = null;
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}


		private void BuildCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true; e.CanExecute = client.IsConnected && (appViewModel?.SelectedItem is PlanetViewModel);
		}

		private void BuildCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			PlanetViewModel planet;
			e.Handled = true;

			try
			{
				planet = (PlanetViewModel)appViewModel.SelectedItem;
				client.BuildFactory(planet.PlanetID,1);
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}



		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (appViewModel == null) return;
			appViewModel.SelectedItem = e.NewValue;
		}


	}
}
