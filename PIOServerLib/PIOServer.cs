using LogLib;
using ModuleLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using NetORMLib.VersionControl;
using PIOServerLib.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIOServerLib
{
	public class PIOServer : ThreadModule
	{
		public IPlanetModule PlanetModule
		{
			get;
			private set;
		}

		private IDatabase database;

		public PIOServer(ILogger Logger) : base("PIOServer", Logger)
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

			//Try(databaseCreator.DropDatabase, "Failed to drop database");

			#region database initialisation
			Log(LogLevels.Information, "Checking if database exists");
			if (!TryGet(databaseCreator.DatabaseExists, out result, "Failed to check database presence")) return false;

			if (!result)
			{
				Log(LogLevels.Information, "Creating database");
				if (!Try(databaseCreator.CreateDatabase, "Failed to create database")) return false;
			}

			Log(LogLevels.Information, "Checking database revision");
			if (!TryGet(versionControl.IsUpToDate, out result, "Failed to check database revision")) return false;

			if (!result)
			{
				Log(LogLevels.Information, $"Upgrading database to revision {versionControl.GetTargetRevision()}");
				if (!Try(versionControl.Upgrade, "Failed to upgrade database")) return false;
			}
			#endregion

			return true;
		}

		// simulate a main: we want to run server as a hosted thread instead of external app
		protected override void ThreadLoop()
		{

			if (!InitializeDatabase()) return;

			PlanetModule = new PlanetModule(Logger, database);

			while (State==ModuleStates.Started)
			{
				WaitHandles(-1, QuitEvent);
			}
		}




	}
}
