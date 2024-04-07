using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib.Exceptions
{
	public abstract class PIOException : Exception
	{
		
		public PIOException(string Message) : base(Message)
		{
			if (Message == null) throw new ArgumentNullException(nameof(Message));
		}

	}
}
