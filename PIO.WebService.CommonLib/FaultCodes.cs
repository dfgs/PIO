using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebService.CommonLib
{
	public static class FaultCodes 
	{
		public static FaultCode NotFound = new FaultCode("NotFound");
		public static FaultCode DataLayerError = new FaultCode("DataLayerError");
	}
}
