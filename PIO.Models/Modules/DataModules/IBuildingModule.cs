using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IBuildingModule : IDatabaseModule
	{
		Building GetBuilding(int BuildingID);
		Building GetBuilding(int X,int Y);
		Building[] GetBuildings(int PlanetID);

		Building CreateBuilding(int PlanetID, int X, int Y, BuildingTypeIDs BuildingTypeID, int RemainingBuildSteps);

		void UpdateBuilding(int BuildingID, int RemainingBuildSteps);

		//void SetHealthPoints(int BuildingID,int HealthPoints);
		/*int CreateBuilding(int PlanetID,int BuildingTypeID,int StateID);
		void SetState(int BuildingID, int StateID);*/
	}
}
