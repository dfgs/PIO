using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Exceptions
{
	public class PIODataException:PIOException
	{
		public PIODataException(string Message,Exception InnerException,int ModuleID,string ModuleName,string MethodName):base(Message,InnerException,ModuleID,ModuleName,MethodName)
		{

		}
	}
}
