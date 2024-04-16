using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib.Exceptions
{
	public class PIOInvalidOperationException : PIOException
	{
		public PIOInvalidOperationException(string Description):base(Description)
		{
			
		}
	}
}
