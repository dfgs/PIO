using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Exceptions
{
	public class PIOInternalErrorException: PIOEngineException
	{
		public override string FaultCode => "InternalError";

		public PIOInternalErrorException(string Message,Exception InnerException,int ModuleID,string ModuleName,string MethodName):base(Message,InnerException,ModuleID,ModuleName,MethodName)
		{

		}
	}
}
