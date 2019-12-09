using LogLib;
using ModuleLib;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib
{
	public class PIOModule : Module, IPIOModule
	{
		public PIOModule(ILogger Logger) : base(Logger)
		{
		}
	}
}
