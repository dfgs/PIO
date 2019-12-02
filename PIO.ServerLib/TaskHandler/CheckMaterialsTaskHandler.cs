using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;
using NetORMLib.Databases;

namespace PIO.ServerLib.TaskHandler
{
	public class CheckMaterialsTaskHandler : TaskHandler
	{
		public override int TaskTypeID => 0;
		public CheckMaterialsTaskHandler(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

	}
}
