using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace PIOServerLib.Modules.Tasks
{
	public class ProgressBuildingTask : Task
	{
		public override int ID => 2;


		public ProgressBuildingTask(ILogger Logger) : base(Logger)
		{
		}

	}
}
