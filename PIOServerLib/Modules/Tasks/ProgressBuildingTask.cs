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
		public override int TaskID => 2;


		public ProgressBuildingTask(ILogger Logger) : base(Logger)
		{
		}

		public override void Enter()
		{
		}

		public override void Leave()
		{
		}
	}
}
