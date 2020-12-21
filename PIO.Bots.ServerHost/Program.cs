using LogLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using PIO.BaseModulesLib.Modules.EngineModules;
using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using PIO.Bots.ServerLib;
using PIO.Bots.ServerLib.Modules;
using PIO.Bots.WebServiceLib;
using PIO.BotsLib;
using PIO.BotsLib.Basic;
using PIO.ClientLib.PIOServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIO.Bots.ServerHost
{
	class Program
	{
		private static AutoResetEvent quitEvent;

		static void Main(string[] args)
		{
			ILogger logger;
			VersionControlModule versionControlModule;
			ServiceHostModule serviceHostModule;
			IBotsService service;


			IDatabase database;
			IConnectionFactory connectionFactory;
			ICommandBuilder commandBuilder;
			IDatabaseCreator databaseCreator;

			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			IOrderManagerModule orderManagerModule;

			IWorkerScheduler workerScheduler;

			PIOServiceClient client;
			Binding binding;
			EndpointAddress remoteAddress;


			quitEvent = new AutoResetEvent(false);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

			//logger = new ConsoleLogger(new DefaultLogFormatter());
			logger = new UnicastLogger(IPAddress.Loopback, Properties.Settings.Default.UnicastPort);

			databaseCreator = new SqlDatabaseCreator(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			connectionFactory = new SqlConnectionFactory(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			commandBuilder = new SqlCommandBuilder();
			database = new Database(connectionFactory, commandBuilder);

			versionControlModule = new VersionControlModule(logger, databaseCreator, new BotsVersionControl(database));
			if (!versionControlModule.InitializeDatabase(Properties.Settings.Default.DropDatabase))
			{
				Console.ReadLine();
				return;
			}

			binding = new BasicHttpBinding();
			remoteAddress = new EndpointAddress($@"http://{Properties.Settings.Default.BotsServerAddress}:8733/Design_Time_Addresses/PIO.WebService/");
			client = new PIOServiceClient(binding, remoteAddress);
			client.Open();



			orderModule = new OrderModule(logger, database);
			produceOrderModule = new ProduceOrderModule(logger, database);

			orderManagerModule = new OrderManagerModule(logger,client, orderModule, produceOrderModule,10);

			service = new BotsService(logger, orderModule,produceOrderModule, orderManagerModule);

			serviceHostModule = new ServiceHostModule(logger, service);
			serviceHostModule.Start();

			workerScheduler = new WorkerScheduler(logger, client, orderManagerModule, 5);
			workerScheduler.Start();
			workerScheduler.Add(1);

			WaitHandle.WaitAny(new WaitHandle[] { quitEvent }, -1);

			serviceHostModule.Stop();
			workerScheduler.Stop();

			client.Close();
			logger.Dispose();

			Console.CancelKeyPress -= new ConsoleCancelEventHandler(Console_CancelKeyPress);

		}


		static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			if (e.SpecialKey == ConsoleSpecialKey.ControlC)
			{
				Console.WriteLine("Control break invoked");
				e.Cancel = true;
				quitEvent.Set();
			}

		}


	}

}
