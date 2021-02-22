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
using PIO.ClientLib;
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

			IBotModule botModule;
			IOrderModule orderModule;
			IOrderManagerModule orderManagerModule;
			IProduceOrderModule produceOrderModule;
			IHarvestOrderModule harvestOrderModule;
			IBuildOrderModule buildOrderModule;

			IBotSchedulerModule botSchedulerModule;

			PIOServiceClient client;


			quitEvent = new AutoResetEvent(false);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

			//logger = new ConsoleLogger(new DefaultLogFormatter());
			logger = new UnicastLogger(IPAddress.Loopback, Properties.Settings.Default.UnicastPort);
			//logger = new FileLogger(new DefaultLogFormatter(), "PIO.Bots.ServerHost.Log");

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

			client = new PIOServiceClient(new BasicHttpBinding(), new EndpointAddress($@"http://127.0.0.1:8733/PIO/Service/"));
			client.Open();


			botModule = new BotModule(logger, database);
			orderModule = new OrderModule(logger, database);
			produceOrderModule = new ProduceOrderModule(logger, database);
			harvestOrderModule = new HarvestOrderModule(logger, database);
			buildOrderModule = new BuildOrderModule(logger, database);

			orderManagerModule = new OrderManagerModule(logger, client, orderModule, produceOrderModule,harvestOrderModule, buildOrderModule, 10) ;

			botSchedulerModule = new BotSchedulerModule(logger, client, botModule, orderManagerModule, 5);
			botSchedulerModule.Start();

			service = new BotsService(logger, botModule, orderModule, produceOrderModule,harvestOrderModule, buildOrderModule, botSchedulerModule, orderManagerModule) ;

			serviceHostModule = new ServiceHostModule(logger, service);
			serviceHostModule.Start();
				
			WaitHandle.WaitAny(new WaitHandle[] { quitEvent }, -1);

			serviceHostModule.Stop();
			botSchedulerModule.Stop();

			client.Close();
			logger.Dispose();

			Console.CancelKeyPress -= new ConsoleCancelEventHandler(Console_CancelKeyPress);

		}


		static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			if (e.SpecialKey == ConsoleSpecialKey.ControlC)
			{
				Console.WriteLine("Control break invoked");
				e.Cancel=true;
				quitEvent.Set();
			}

		}


	}

}
