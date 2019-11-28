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
	public class VersionControlModule : Module,IVersionControlModule
	{
		private string server;
		
		public VersionControlModule(ILogger Logger,string Server) : base( Logger)
		{
			this.server = Server;
		}

		public bool InitializeDatabase(bool DropDatabase )
		{
			bool result;

			IDatabase database;
			IDatabaseCreator databaseCreator;
			//databaseCreator = new SqlLocalDatabaseCreator("PIO", Directory.GetCurrentDirectory());
			databaseCreator = new SqlDatabaseCreator(server, "PIO");

			IConnectionFactory connectionFactory;
			//connectionFactory = new SqlLocalConnectionFactory(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PIO.mdf"));
			connectionFactory = new SqlConnectionFactory(server, "PIO");

			ICommandBuilder commandBuilder;
			commandBuilder = new SqlCommandBuilder();

			database = new Database(connectionFactory, commandBuilder);

			IVersionControl versionControl;
			versionControl = new PIOVersionControl(database);

			if (DropDatabase)
			{
				Log(LogLevels.Information, "Dropping database if exists");
				if (!Try(databaseCreator.DropDatabase).OrAlert("Failed to drop database")) return false;
			}

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





	}
}
