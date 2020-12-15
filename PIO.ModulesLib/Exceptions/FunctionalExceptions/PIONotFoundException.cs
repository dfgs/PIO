using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib.Exceptions
{
	public class PIONotFoundException: PIOFunctionalException
	{
		public override string FaultCode => "NotFound";
		public PIONotFoundException(string Message,Exception InnerException,int ModuleID,string ModuleName,string MethodName):base(Message,InnerException,ModuleID,ModuleName,MethodName)
		{

		}
	}
}
