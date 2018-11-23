using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Tasks
{
	public abstract class Task : Module, ITask
	{
		public abstract int TaskID
		{
			get;
		}

		public Task(ILogger Logger) : base(Logger)
		{
		}

		public abstract void Enter();
		public abstract void Leave();
	}
}
