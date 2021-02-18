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
		bool HasEnoughResourcesToProduce(int BuildingID);
		ResourceTypeIDs[] GetMissingResourcesToProduce(int BuildingID);
		bool HasEnoughResourcesToBuild(int BuildingID);
		ResourceTypeIDs[] GetMissingResourcesToBuild(int BuildingID);
	}
}
