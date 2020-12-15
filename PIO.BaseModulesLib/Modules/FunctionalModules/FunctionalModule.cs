using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib;
using PIO.BaseModulesLib;
using PIO.ModulesLib.Modules.FunctionalModules;

namespace PIOBaseModulesLib.Modules.FunctionalModules
{
	public abstract class FunctionalModule : PIOModule, IFunctionalModule
	{
		protected FunctionalModule(ILogger Logger) : base(Logger)
		{
		}

		/*protected void ThrowFunctionalException(string Message,[CallerMemberName]string MethodName=null)
		{
			Log(LogLevels.Warning, Message);
			throw new PIOFunctionalException(Message, null, ID, ModuleName, MethodName);
		}*/
	}

	
}
