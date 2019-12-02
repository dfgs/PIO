using LogLib;
using NetORMLib.Databases;
using PIO.ServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.TaskHandler
{
	public abstract class TaskHandler : DatabaseModule, ITaskHandler
	{
		public abstract int TaskTypeID { get; }

		public TaskHandler(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

	}
}
