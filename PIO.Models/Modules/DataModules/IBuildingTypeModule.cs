using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface IBuildingTypeModule:IDatabaseModule
	{
		BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID);
		BuildingType[] GetBuildingTypes();

		BuildingType CreateBuildingType(BuildingTypeIDs BuildingTypeID, string Name, int BuildSteps, int HealthPoints);
	}
}
