using PIO.ModulesLib.Modules.FunctionalModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IPlanetGeneratorModule: IFunctionalModule
	{
		bool Generate();
	}
}
