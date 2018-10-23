using LogLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using NetORMLib.VersionControl;
using PIOServerLib;
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
	
		private PIOServer server;

		public MainWindow()
		{
			InitializeComponent();

			logger = new ConsoleLogger(new DefaultLogFormatter());

			IDatabaseCreator databaseCreator;
			databaseCreator = new SqlLocalDatabaseCreator("PIO", Directory.GetCurrentDirectory());

			IConnectionFactory connectionFactory;
			connectionFactory = new SqlLocalConnectionFactory(System.IO.Path.Combine( Directory.GetCurrentDirectory(),"PIO.mdf" ));

			ICommandBuilder commandBuilder;
			commandBuilder = new SqlCommandBuilder();

			IDatabase database;
			database = new Database(connectionFactory, commandBuilder);

			IVersionControl versionControl;
			versionControl = new PIOVersionControl(database);

			server = new PIOServer(logger,databaseCreator,versionControl);
			server.Start();
		}
		
		protected override void OnClosing(CancelEventArgs e)
		{
			server.Stop();
			base.OnClosing(e);
		}


	}
}
