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
using System.Threading;

namespace PIO.ServerLib.Modules
{
	public class SchedulerModule : AtModule<Task>,ISchedulerModule
	{

		public SchedulerModule(ILogger Logger) : base(Logger, ThreadPriority.Normal)
		{
		}

		public void Add(Task Task)
		{
			this.Add(Task.ETA, Task);
		}

		protected override void OnTriggerEvent(Task Event)
		{
			Log(LogLevels.Information, $"Task finished (TaskID={Event.TaskID})");
		}
	}
}
