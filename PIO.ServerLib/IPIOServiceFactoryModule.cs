using PIO.WebServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib
{
	public interface IPIOServiceFactoryModule
	{
		IPIOService CreateService();
	}
}
