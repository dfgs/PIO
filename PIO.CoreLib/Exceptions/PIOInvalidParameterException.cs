using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib.Exceptions
{
	public class PIOInvalidParameterException : PIOException
	{
		public PIOInvalidParameterException(string ParameterName):base($"Invalid parameter value for {ParameterName}")
		{
			if (ParameterName == null) throw new ArgumentNullException(nameof(ParameterName));
		}
	}
}
