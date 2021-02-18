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
	public interface IFarmModule : IDatabaseModule
	{
		Farm GetFarm(int FarmID);
		Farm GetFarm(int PlanetID, int X, int Y);
		Farm[] GetFarms(int PlanetID);

		//void SetHealthPoints(int FarmID,int HealthPoints);
		Farm CreateFarm(int PlanetID, int X, int Y, int RemainingBuildSteps, BuildingTypeIDs BuildingTypeID, FarmTypeIDs FarmTypeID);

	}
}
