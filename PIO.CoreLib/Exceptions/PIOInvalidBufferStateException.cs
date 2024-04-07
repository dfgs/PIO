using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib.Exceptions
{
	public class PIOInvalidBufferStateException : PIOException
	{
		public PIOInvalidBufferStateException():base("Buffer state is invalid")
		{
		}
	}
}
