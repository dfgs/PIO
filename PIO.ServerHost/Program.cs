using LogLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using PIO.BaseModulesLib.Modules.EngineModules;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ServerLib;
using PIO.ServerLib.Modules;
using PIO.WebServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
			SchedulerModule schedulerModule;

			IPIOService service;
			IDatabase database;
			IConnectionFactory connectionFactory;
			ICommandBuilder commandBuilder;
			IDatabaseCreator databaseCreator;

			IPlanetModule planetModule;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IFactoryBuilderModule factoryBuilderModule;
			IStackModule stackModule;
			IResourceTypeModule resourceTypeModule;
			IBuildingTypeModule buildingTypeModule;
			IFactoryTypeModule factoryTypeModule;
			ITaskTypeModule taskTypeModule;
			IMaterialModule materialModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;

			IIdlerModule idlerModule;
			IResourceCheckerModule resourceCheckerModule;
			ILocationCheckerModule locationCheckerModule;
			IProducerModule producerModule;
			IMoverModule moverModule;
			ICarrierModule carrierModule;

			quitEvent = new AutoResetEvent(false);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

			//logger = new ConsoleLogger(new DefaultLogFormatter());
			logger = new UnicastLogger(IPAddress.Loopback, Properties.Settings.Default.UnicastPort);

			databaseCreator = new SqlDatabaseCreator(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			connectionFactory = new SqlConnectionFactory(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			commandBuilder = new SqlCommandBuilder();
			database = new Database(connectionFactory, commandBuilder);

			versionControlModule = new VersionControlModule(logger,databaseCreator,new PIOVersionControl(database));
			if (!versionControlModule.InitializeDatabase(Properties.Settings.Default.DropDatabase))
			{
				Console.ReadLine();
				return;
			}


			planetModule = new PlanetModule(logger, database);
			buildingModule = new BuildingModule(logger, database);
			factoryModule = new FactoryModule(logger, database);
			workerModule = new WorkerModule(logger, database);
			stackModule = new StackModule(logger, database);
			resourceTypeModule = new ResourceTypeModule(logger, database);
			buildingTypeModule = new BuildingTypeModule(logger, database);
			factoryTypeModule = new FactoryTypeModule(logger, database);
			taskTypeModule = new TaskTypeModule(logger, database);
			materialModule = new MaterialModule(logger, database);
			ingredientModule = new IngredientModule(logger, database);
			productModule = new ProductModule(logger, database);
			taskModule = new TaskModule(logger, database);


			factoryBuilderModule = new FactoryBuilderModule(logger,taskModule,workerModule,buildingModule, factoryModule, factoryTypeModule, stackModule,materialModule);
			idlerModule = new IdlerModule(logger,taskModule, workerModule);
			resourceCheckerModule = new ResourceCheckerModule(logger, factoryModule, stackModule, ingredientModule);
			locationCheckerModule = new LocationCheckerModule(logger, workerModule, buildingModule, factoryModule);
			producerModule = new ProducerModule(logger, taskModule, workerModule,buildingModule, factoryModule,  stackModule, ingredientModule, productModule);
			moverModule = new MoverModule(logger, taskModule, workerModule, factoryModule);
			carrierModule = new CarrierModule(logger, taskModule, workerModule,buildingModule, factoryModule, stackModule);

			schedulerModule = new SchedulerModule(logger, taskModule, idlerModule, producerModule,moverModule,carrierModule, factoryBuilderModule);
			schedulerModule.Start();
			

			service = new PIOService(
				logger,planetModule,buildingModule,factoryModule,workerModule,
				stackModule,resourceTypeModule,
				buildingTypeModule,factoryTypeModule,taskTypeModule,materialModule,ingredientModule,productModule,taskModule, 
				resourceCheckerModule,locationCheckerModule, idlerModule,producerModule,moverModule,carrierModule,factoryBuilderModule);

			serviceHostModule = new ServiceHostModule(logger,service);
			serviceHostModule.Start();

			

			WaitHandle.WaitAny(new WaitHandle[] {quitEvent }, -1);

			serviceHostModule.Stop();
			schedulerModule.Stop();

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
