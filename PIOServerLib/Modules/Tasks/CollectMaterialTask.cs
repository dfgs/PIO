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
		public override int ID => 1;


		public CollectMaterialTask(ILogger Logger) : base(Logger)
		{
		}

	}
}
