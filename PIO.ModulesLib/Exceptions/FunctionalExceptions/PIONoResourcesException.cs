using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib.Exceptions
{
	public class PIONoResourcesException : PIOFunctionalException
	{
		public override string FaultCode => "NoResources";
		public PIONoResourcesException(string Message,Exception InnerException,int ModuleID,string ModuleName,string MethodName):base(Message,InnerException,ModuleID,ModuleName,MethodName)
		{

		}
	}
}
