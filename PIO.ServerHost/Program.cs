using LogLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using PIO.Models.Modules;
using PIO.ServerLib;
using PIO.ServerLib.Modules;
using PIO.ServerLib.TaskHandler;
using PIO.WebServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIO.ServerHost
{
	class Program
	{
		private static AutoResetEvent quitEvent;

		static void Main(string[] args)
		{
			ILogger logger;
			VersionControlModule versionControlModule;
			ServiceHostModule serviceHostModule;
			TaskSchedulerModule taskSchedulerModule;

			IPIOService service;
			IDatabase database;
			IConnectionFactory connectionFactory;
			ICommandBuilder commandBuilder;
			IDatabaseCreator databaseCreator;

			IPlanetModule planetModule;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IResourceTypeModule resourceTypeModule;
			IFactoryTypeModule factoryTypeModule;
			IMaterialModule materialModule;
			ITaskTypeModule taskTypeModule;
			ITaskModule taskModule;


			quitEvent = new AutoResetEvent(false);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

			logger = new ConsoleLogger(new DefaultLogFormatter());

			databaseCreator = new SqlDatabaseCreator(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			connectionFactory = new SqlConnectionFactory(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			commandBuilder = new SqlCommandBuilder();
			database = new Database(connectionFactory, commandBuilder);

			versionControlModule = new VersionControlModule(logger,databaseCreator,database);
			if (!versionControlModule.InitializeDatabase(Properties.Settings.Default.DropDatabase))
			{
				Console.ReadLine();
				return;
			}

			planetModule = new PlanetModule(logger, database);
			factoryModule = new FactoryModule(logger, database);
			stackModule = new StackModule(logger, database);
			resourceTypeModule = new ResourceTypeModule(logger, database);
			factoryTypeModule = new FactoryTypeModule(logger, database);
			materialModule = new MaterialModule(logger, database);
			taskTypeModule = new TaskTypeModule(logger, database);
			taskModule = new TaskModule(logger, database);

			service = new PIOService(planetModule,factoryModule,stackModule,resourceTypeModule,factoryTypeModule,materialModule,taskTypeModule,taskModule);

			serviceHostModule = new ServiceHostModule(logger,service);
			serviceHostModule.Start();

			taskSchedulerModule = new TaskSchedulerModule(logger,taskModule);
			taskSchedulerModule.Register(new CheckMaterialsTaskHandler(logger,factoryModule,stackModule,materialModule));
			if (taskSchedulerModule.Initialize()) taskSchedulerModule.Start();

			WaitHandle.WaitAny(new WaitHandle[] {quitEvent }, -1);

			taskSchedulerModule.Stop();
			serviceHostModule.Stop();

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
