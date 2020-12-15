using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedBuildingModule : MockedDatabaseModule, IBuildingModule
	{
		public MockedBuildingModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public Building CreateBuilding(int PlanetID, int X, int Y, BuildingTypeIDs BuildingTypeID, int RemainingBuildingSteps)
		{
			throw new NotImplementedException();
		}

		public Building GetBuilding(int BuildingID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Building() { BuildingID = BuildingID };
		}
		public Building GetBuilding(int PlanetID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Building() { X=X,Y=Y };
		}

		/*public Building[] GetBuildings()
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Building() { BuildingID = t });
		}*/

		public Building[] GetBuildings(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Building() { BuildingID = t,PlanetID=PlanetID });
		}

		public void UpdateBuilding(int BuildingID, int RemainingBuildSteps)
		{
			throw (new NotImplementedException());
		}

	}
}
