using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Executors
{
	public class ScheduledExecutor
	{
		public IExecutor Executor
		{
			get;
			set;
		}
		public int FactoryID
		{
			get;
			set;
		}
	}
}
