using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using NetORMLib.VersionControl;
using PIO.Models;
using PIO.ServerLib.Modules;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace PIO.ServerLib
{
	public class PIOServer : ThreadModule,IPIOServer
	{
		private IMessageBrokerModule MessageBrokerModule;
		private ITaskSchedulerModule TaskSchedulerModule;
		private IFSMModule FSMModule;

		private IPlanetModule PlanetModule;
		private IFactoryModule FactoryModule;
		private IStackModule StackModule;
		private ITaskModule TaskModule;
		private IStateModule StateModule;
		private ITransitionModule TransitionModule;
		private IEventModule EventModule;
		private IScheduledTaskModule ScheduledTaskModule;

		private IDatabase database;

		public bool IsInitialized
		{
			get;
			private set;
		}

		public PIOServer(ILogger Logger) : base( Logger)
		{
		}

		private bool InitializeDatabase()
		{
			bool result;

			IDatabaseCreator databaseCreator;
			//databaseCreator = new SqlLocalDatabaseCreator("PIO", Directory.GetCurrentDirectory());
			databaseCreator = new SqlDatabaseCreator("127.0.0.1", "PIO");

			IConnectionFactory connectionFactory;
			//connectionFactory = new SqlLocalConnectionFactory(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PIO.mdf"));
			connectionFactory = new SqlConnectionFactory("127.0.0.1","PIO");

			ICommandBuilder commandBuilder;
			commandBuilder = new SqlCommandBuilder();

			database = new Database(connectionFactory, commandBuilder);

			IVersionControl versionControl;
			versionControl = new PIOVersionControl(database);

			Try(databaseCreator.DropDatabase).OrAlert("Failed to drop database");

			#region database initialisation
			Log(LogLevels.Information, "Checking if database exists");
			if (!Try(databaseCreator.DatabaseExists).OrAlert(out result, "Failed to check database presence")) return false;

			if (!result)
			{
				Log(LogLevels.Information, "Creating database");
				if (!Try(databaseCreator.CreateDatabase).OrAlert("Failed to create database")) return false;
				//Thread.Sleep(5000);
			}

			Log(LogLevels.Information, "Checking database revision");
			if (!Try(versionControl.IsUpToDate).OrAlert(out result, "Failed to check database revision")) return false;

			if (!result)
			{
				Log(LogLevels.Information, $"Upgrading database to revision {versionControl.GetTargetRevision()}");
				if (!Try(versionControl.Upgrade).OrAlert("Failed to upgrade database")) return false;
			}
			#endregion



			return true;
		}


	


		// simulate a main: we want to run server as a hosted thread instead of external app
		protected override void ThreadLoop()
		{

			if (!InitializeDatabase()) return;

			PlanetModule = new PlanetModule(Logger, database);
			FactoryModule = new FactoryModule(Logger, database);
			StackModule = new StackModule(Logger, database);
			TaskModule = new TaskModule(Logger, database);
			StateModule = new StateModule(Logger, database);
			TransitionModule = new TransitionModule(Logger, database);
			EventModule = new EventModule(Logger, database);
			ScheduledTaskModule = new ScheduledTaskModule(Logger, database);

			MessageBrokerModule = new MessageBrokerModule(Logger,EventModule);
			TaskSchedulerModule = new TaskSchedulerModule(Logger,ScheduledTaskModule,MessageBrokerModule);
			FSMModule = new FSMModule(Logger,  FactoryModule, StateModule,TransitionModule, TaskSchedulerModule);

			MessageBrokerModule.Subscribe(FSMModule);

			TaskSchedulerModule.Start();
			IsInitialized = true;


			while (State==ModuleStates.Started)
			{
				WaitHandles(-1, QuitEvent);
			}

			TaskSchedulerModule.Stop();
		}

		public Planet GetPlanet(int PlanetID)
		{
			return PlanetModule.GetPlanet(PlanetID);
		}

		public IEnumerable<Planet> GetPlanets()
		{
			return PlanetModule.GetPlanets();
		}

		public IEnumerable<Factory> GetFactories(int PlanetID)
		{
			return FactoryModule.GetFactories(PlanetID);
		}

		public IEnumerable<Stack> GetStacks(int FactoryID)
		{
			return StackModule.GetStacks(FactoryID);
		}

		public Task GetTask(int TaskID)
		{
			return TaskModule.GetTask(TaskID);
		}

		public State GetState(int StateID)
		{
			return StateModule.GetState(StateID);
		}

		public Factory BuildFactory(int PlanetID, int FactoryTypeID)
		{
			dynamic item;
			LogEnter();

			item = new Factory();
			item.PlanetID = PlanetID;
			item.Name = "New";
			item.FactoryID = Try(() => FactoryModule.CreateFactory(PlanetID, FactoryTypeID,0)).OrThrow("Failed to build factory");

			Try(() => { FSMModule.Initialize(item.FactoryID, 0); }).OrAlert("Failed to initialize factory state");

			return item;
		}




	}
}
