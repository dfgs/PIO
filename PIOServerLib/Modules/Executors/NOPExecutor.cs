using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Executors
{
	public class NOPExecutor : Executor
	{
		public override int TaskID => 0;

		public NOPExecutor(ILogger Logger):base(Logger)
		{

		}

		public override int Execute(int FactoryID)
		{
			Log(LogLevels.Information, $"NOP FactoryID {FactoryID}");
			return 1;
		}
	}
}
