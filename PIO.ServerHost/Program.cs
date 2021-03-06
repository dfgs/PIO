﻿using LogLib;
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
			ServiceHostModule pioServiceHostModule;
			ServiceHostModule taskCallbackServiceHostModule;
			SchedulerModule schedulerModule;

			IPIOService pioService;
			ITaskCallbackService taskCallbackService;
			IDatabase database;
			IConnectionFactory connectionFactory;
			ICommandBuilder commandBuilder;
			IDatabaseCreator databaseCreator;

			IPlanetGeneratorModule planetGeneratorModule;

			IPhraseModule phraseModule;
			IPlanetModule planetModule;
			ICellModule cellModule;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IBuilderModule factoryBuilderModule;
			IStackModule stackModule;
			IResourceTypeModule resourceTypeModule;
			IBuildingTypeModule buildingTypeModule;
			ITaskTypeModule taskTypeModule;
			IMaterialModule materialModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;

			IIdlerModule idlerModule;
			IResourceCheckerModule resourceCheckerModule;
			ILocationCheckerModule locationCheckerModule;
			IProducerModule producerModule;
			IHarvesterModule harvesterModule;
			IMoverModule moverModule;
			ITakerModule takerModule;
			IStorerModule storerModule;

			quitEvent = new AutoResetEvent(false);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

			//logger = new ConsoleLogger(new DefaultLogFormatter());
			logger = new UnicastLogger(IPAddress.Loopback, Properties.Settings.Default.UnicastPort);
			//logger = new FileLogger(new DefaultLogFormatter(), "PIO.ServerHost.Log");

			databaseCreator = new SqlDatabaseCreator(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			connectionFactory = new SqlConnectionFactory(Properties.Settings.Default.Server, Properties.Settings.Default.DatabaseName);
			commandBuilder = new SqlCommandBuilder();
			database = new Database(connectionFactory, commandBuilder);

			versionControlModule = new VersionControlModule(logger,databaseCreator,new PIOVersionControl(database));
			if (!versionControlModule.InitializeDatabase(Properties.Settings.Default.DropDatabase)) return;


			phraseModule = new PhraseModule(logger, database);
			planetModule = new PlanetModule(logger, database);
			cellModule = new CellModule(logger, database);
			buildingModule = new BuildingModule(logger, database);
			workerModule = new WorkerModule(logger, database);
			stackModule = new StackModule(logger, database);
			resourceTypeModule = new ResourceTypeModule(logger, database);
			buildingTypeModule = new BuildingTypeModule(logger, database);
			taskTypeModule = new TaskTypeModule(logger, database);
			materialModule = new MaterialModule(logger, database);
			ingredientModule = new IngredientModule(logger, database);
			productModule = new ProductModule(logger, database);
			taskModule = new TaskModule(logger, database);


			factoryBuilderModule = new BuilderModule(logger, taskModule, workerModule, buildingModule, buildingTypeModule, stackModule, materialModule); ;
			idlerModule = new IdlerModule(logger,taskModule, workerModule);
			resourceCheckerModule = new ResourceCheckerModule(logger, buildingModule,stackModule, ingredientModule,materialModule);
			locationCheckerModule = new LocationCheckerModule(logger, workerModule, buildingModule);
			producerModule = new ProducerModule(logger, taskModule, workerModule, buildingModule, buildingTypeModule,  stackModule, ingredientModule, productModule);
			harvesterModule = new HarvesterModule(logger, taskModule, workerModule, buildingModule, buildingTypeModule, stackModule, productModule);
			moverModule = new MoverModule(logger, taskModule, workerModule, buildingModule);
			takerModule = new TakerModule(logger, taskModule, workerModule, buildingModule, stackModule);
			storerModule = new StorerModule(logger, taskModule, workerModule, buildingModule, stackModule);

			planetGeneratorModule = new PlanetGeneratorModule(logger,phraseModule, resourceTypeModule,buildingTypeModule,taskTypeModule,materialModule,ingredientModule,productModule,planetModule,cellModule,buildingModule,workerModule);
			if (!planetGeneratorModule.Generate()) return; 

			schedulerModule = new SchedulerModule(logger, taskModule, idlerModule, producerModule,harvesterModule, moverModule,takerModule,storerModule, factoryBuilderModule);
			schedulerModule.Start();


			pioService = new PIOService(
				logger,phraseModule, planetModule, cellModule, buildingModule, workerModule,
				stackModule, resourceTypeModule,
				 buildingTypeModule, taskTypeModule, materialModule, ingredientModule, productModule, taskModule,
				schedulerModule,
				resourceCheckerModule, locationCheckerModule, idlerModule, producerModule, harvesterModule, moverModule,  takerModule, storerModule,
				factoryBuilderModule) ; 

			pioServiceHostModule = new ServiceHostModule(logger,pioService);
			pioServiceHostModule.Start();

			taskCallbackService = new TaskCallbackService(logger, schedulerModule);
			taskCallbackServiceHostModule = new ServiceHostModule(logger, taskCallbackService);
			taskCallbackServiceHostModule.Start();

			WaitHandle.WaitAny(new WaitHandle[] {quitEvent }, -1);

			taskCallbackServiceHostModule.Stop();
			pioServiceHostModule.Stop();
			schedulerModule.Stop();

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
