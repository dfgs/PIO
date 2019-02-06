using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Executors
{
	public struct ScheduledExecutor
	{
		public IExecutor Executor
		{
			get;
			private set;
		}
		public int ScheduledTaskID
		{
			get;
			private set;
		}
		public int FactoryID
		{
			get;
			private set;
		}

		public ScheduledExecutor(IExecutor Executor, int ScheduledTaskID, int FactoryID)
		{
			this.Executor = Executor;this.ScheduledTaskID = ScheduledTaskID;this.FactoryID = FactoryID;
		}
	}
}
