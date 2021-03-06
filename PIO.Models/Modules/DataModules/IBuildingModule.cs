﻿using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface IBuildingModule : IDatabaseModule
	{
		Building GetBuilding(int BuildingID);
		Building GetBuilding(int PlanetID, int X,int Y);
		Building[] GetBuildings(int PlanetID);

		Building CreateBuilding(int PlanetID, int X, int Y, BuildingTypeIDs BuildingTypeID, int RemainingBuildSteps, int HealthPoints);

		void UpdateBuilding(int BuildingID, int RemainingBuildSteps);

		
	}
}
