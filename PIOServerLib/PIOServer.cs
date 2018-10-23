using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using NetORMLib.VersionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib
{
	public class PIOServer : ThreadModule
	{
		private IDatabaseCreator databaseCreator;
		private IVersionControl versionControl;

		public PIOServer(ILogger Logger,IDatabaseCreator DatabaseCreator, IVersionControl VersionControl) : base("PIOServer", Logger)
		{
			this.databaseCreator = DatabaseCreator;
			this.versionControl = VersionControl;
		}

		protected override void ThreadLoop()
		{
			bool result;

			#region database initialisation
			Log(LogLevels.Information, "Checking if database exists");
			if (!TryGet(databaseCreator.DatabaseExists, out result, "Failed to check database presence")) return;

			if (!result)
			{
				Log(LogLevels.Information, "Creating database");
				if (!Try(databaseCreator.CreateDatabase,"Failed to create database")) return;
			}

			Log(LogLevels.Information, "Checking database revision");
			if (!TryGet(versionControl.IsUpToDate, out result, "Failed to check database revision")) return;

			if (!result)
			{
				Log(LogLevels.Information, $"Upgrading database to revision {versionControl.GetTargetRevision()}");
				if (!Try(versionControl.Upgrade, "Failed to upgrade database")) return;
			}
			#endregion

			while (State==ModuleStates.Started)
			{
				WaitHandles(-1, QuitEvent);
			}
		}




	}
}
