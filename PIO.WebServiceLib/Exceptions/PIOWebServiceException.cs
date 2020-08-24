using PIO.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebServiceLib.Exceptions
{
	public class PIOWebServiceException : PIOEngineException
	{
		public PIOWebServiceException(string Message, Exception InnerException, int ModuleID, string ModuleName, string MethodName) : base(Message, InnerException, ModuleID, ModuleName, MethodName)
		{
		}
	}
}
