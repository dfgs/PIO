using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.FunctionalModules;

namespace PIO.Models.Modules
{
	public interface IResourceCheckerModule : IFunctionalModule
	{
		bool HasEnoughResourcesToProduce(int FactoryID);
		ResourceTypeIDs[] GetMissingResourcesToProduce(int FactoryID);
		bool HasEnoughResourcesToBuild(int FactoryID);
		ResourceTypeIDs[] GetMissingResourcesToBuild(int FactoryID);
	}
}
