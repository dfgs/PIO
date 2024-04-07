using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib.Exceptions
{
	public abstract class PIOException : Exception
	{
		protected PIOException()
		{
		}

		protected PIOException(string Message) : base(Message)
		{
		}

	}
}
