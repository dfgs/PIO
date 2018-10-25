using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOClientLib
{
	public class PIOClientException:Exception
	{
		public PIOClientException(string Message, Exception InnerException) :base(Message, InnerException)
		{

		}
	}
}
