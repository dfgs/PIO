using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib.Exceptions
{
	public class PIOInvalidOperationException: PIOFunctionalException
	{
		public override string FaultCode => "InvalidOperation";
		public PIOInvalidOperationException(string Message,Exception InnerException,int ModuleID,string ModuleName,string MethodName):base(Message,InnerException,ModuleID,ModuleName,MethodName)
		{

		}
	}
}
