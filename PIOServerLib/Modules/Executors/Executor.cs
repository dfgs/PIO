using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Executors
{
	public abstract class Executor : Module, IExecutor
	{
		public abstract int TaskID
		{
			get;
		}

		public Executor(ILogger Logger) : base(Logger)
		{
		}

		public abstract int Execute(int FactoryID);
	}
}
