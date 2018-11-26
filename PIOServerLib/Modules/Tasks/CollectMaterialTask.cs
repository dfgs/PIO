using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace PIOServerLib.Modules.Tasks
{
	public class CollectMaterialTask : Task
	{
		public override int TaskID => 1;


		public CollectMaterialTask(ILogger Logger) : base(Logger)
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
