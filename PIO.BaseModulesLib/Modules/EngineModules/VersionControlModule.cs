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
using PIO.ModulesLib.Modules.EngineModules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace PIO.BaseModulesLib.Modules.EngineModules
{
	public class VersionControlModule : Module,IVersionControlModule
	{
		private IVersionControl versionControl;
		private IDatabaseCreator databaseCreator;

		public VersionControlModule(ILogger Logger, IDatabaseCreator DatabaseCreator, IVersionControl VersionControl) : base( Logger)
		{
			this.databaseCreator = DatabaseCreator;
			this.versionControl = VersionControl;
		}

		public bool InitializeDatabase(bool DropDatabase )
		{
			bool result;

			
			if (DropDatabase)
			{
				Log(LogLevels.Information, "Dropping database if exists");
				//databaseCreator.DatabaseExists
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
