using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedBuildingModule :MockedDatabaseModule<Building>, IBuildingModule
	{

		public MockedBuildingModule(bool ThrowException, params Building[] Items):base(ThrowException,Items)
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

		public Building CreateBuilding(int PlanetID, BuildingTypeIDs BuildingTypeID, int RemainingBuildSteps)
		{
			Building result;

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			result = new Building() { BuildingID = items.Count };
			items.Add(result);
			return result;
		}


	}
}
