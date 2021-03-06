﻿using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedBuildingModule : MockedDatabaseModule<Building>, IBuildingModule
	{
	

		public MockedBuildingModule(bool ThrowException, params Building[] Items) : base(ThrowException, Items)
		{
		}

		public Building[] GetBuildings(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.BuildingID == PlanetID).ToArray();
		}

		public Building GetBuilding(int BuildingID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.BuildingID == BuildingID);
		}
		public Building GetBuilding(int PlanetID, int X, int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => (item.X == X) && (item.Y == Y));
		}


		public void UpdateBuilding(int BuildingID, int RemainingBuildSteps)
		{
			items.FirstOrDefault(item => item.BuildingID == BuildingID).RemainingBuildSteps = RemainingBuildSteps;
		}

		/*public Building CreateBuilding(int PlanetID, int X, int Y, int RemainingBuildSteps, BuildingTypeIDs BuildingTypeID)
		{
			Building result;

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			result = new Building() { BuildingID = items.Count,  PlanetID = PlanetID, X = X, Y = Y, RemainingBuildSteps = RemainingBuildSteps, BuildingTypeID = BuildingTypeID };
			items.Add(result);
			return result;
		}*/

		public Building CreateBuilding(int PlanetID, int X, int Y, BuildingTypeIDs BuildingTypeID, int RemainingBuildSteps, int HealthPoints)
		{
			Building result;

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			result = new Building() { BuildingID = items.Count, PlanetID = PlanetID, X = X, Y = Y };
			items.Add(result);
			return result;
		}
	}

}
