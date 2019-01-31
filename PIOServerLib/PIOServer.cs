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
using PIOServerLib.Modules;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIOServerLib
{
	public class PIOServer : ThreadModule,IPIOServer
	{
		private ITaskSchedulerModule TaskSchedulerModule;
		private IPlanetModule PlanetModule;
		private IFactoryModule FactoryModule;
		private IStackModule StackModule;
		private ITaskModule TaskModule;
		private IStateModule StateModule;

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
			databaseCreator = new SqlLocalDatabaseCreator("PIO", Directory.GetCurrentDirectory());

			IConnectionFactory connectionFactory;
			connectionFactory = new SqlLocalConnectionFactory(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PIO.mdf"));

			ICommandBuilder commandBuilder;
			commandBuilder = new SqlCommandBuilder();

			database = new Database(connectionFactory, commandBuilder);

			IVersionControl versionControl;
			versionControl = new PIOVersionControl(database);

			Try(databaseCreator.DropDatabase).OrLog("Failed to drop database");

			#region database initialisation
			Log(LogLevels.Information, "Checking if database exists");
			if (!Try(databaseCreator.DatabaseExists).OrLog(out result, "Failed to check database presence")) return false;

			if (!result)
			{
				Log(LogLevels.Information, "Creating database");
				if (!Try(databaseCreator.CreateDatabase).OrLog("Failed to create database")) return false;
				Thread.Sleep(5000);
			}

			Log(LogLevels.Information, "Checking database revision");
			if (!Try(versionControl.IsUpToDate).OrLog(out result, "Failed to check database revision")) return false;

			if (!result)
			{
				Log(LogLevels.Information, $"Upgrading database to revision {versionControl.GetTargetRevision()}");
				if (!Try(versionControl.Upgrade).OrLog("Failed to upgrade database")) return false;
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

			TaskSchedulerModule = new TaskSchedulerModule(Logger, FactoryModule, TaskModule);

			IsInitialized = true;

			while (State==ModuleStates.Started)
			{
				WaitHandles(-1, QuitEvent);
			}
		}

		public Row GetPlanet(int PlanetID)
		{
			return PlanetModule.GetPlanet(PlanetID);
		}

		public IEnumerable<Row> GetPlanets()
		{
			return PlanetModule.GetPlanets();
		}

		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			return FactoryModule.GetFactories(PlanetID);
		}

		public IEnumerable<Row> GetStacks(int FactoryID)
		{
			return StackModule.GetStacks(FactoryID);
		}

		public IEnumerable<Row> GetTasks(int FactoryID)
		{
			return TaskModule.GetTasks(FactoryID);
		}

		public Row GetState(int StateID)
		{
			return StateModule.GetState(StateID);
		}

		public Row BuildFactory(int PlanetID, int FactoryTypeID)
		{
			dynamic item;
			LogEnter();

			item = new Row(Table<Factory>.Columns);
			item.PlanetID = PlanetID;
			item.Name = "New";
			item.FactoryID = Try(() => FactoryModule.CreateFactory(PlanetID, FactoryTypeID)).OrThrow("Failed to build factory");

			Try( ()=> { TaskSchedulerModule.SetTask(item.FactoryID, 1); } ).OrThrow("Failed to initialize factory task");

			return item;
		}




	}
}
