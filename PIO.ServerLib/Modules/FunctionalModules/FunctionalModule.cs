using LogLib;
using PIO.Models.Modules;
using PIO.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public abstract class FunctionalModule : PIOModule, IFunctionalModule
	{
		protected FunctionalModule(ILogger Logger) : base(Logger)
		{
		}

		protected void ThrowFunctionalException(string Message,[CallerMemberName]string Caller=null)
		{
			Log(LogLevels.Warning, Message);
			throw new PIOFunctionalException(Message, null, ID, ModuleName, Caller);
		}
	}
}
