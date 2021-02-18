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

		public Building GetBuilding(int BuildingID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Building() { BuildingID = BuildingID };
		}
		public Building GetBuilding(int PlanetID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Building() ;
		}

		

		public Building[] GetBuildings(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Building() { BuildingID = t,PlanetID=PlanetID });
		}

		public void SetHealthPoints(int BuildingID, int HealthPoints)
		{
			throw new NotImplementedException();
		}

		
		public Building CreateBuilding(int PlanetID, int X, int Y, int RemainingBuildSteps, BuildingTypeIDs BuildingTypeID)
		{
			throw new NotImplementedException();
		}

		public void UpdateBuilding(int BuildingID, int RemainingBuildSteps)
		{
			throw new NotImplementedException();
		}
	}
}
