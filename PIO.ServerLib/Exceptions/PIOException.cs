using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Exceptions
{
	public class PIOException:Exception
	{
		public PIOException(string Message):base(Message)
		{

		}
	}
}
