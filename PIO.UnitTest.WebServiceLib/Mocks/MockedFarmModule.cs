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
	public class MockedFarmModule : MockedDatabaseModule, IFarmModule
	{
		public MockedFarmModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public Farm GetFarm(int FarmID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Farm() { FarmID = FarmID };
		}
		public Farm GetFarm(int PlanetID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Farm() ;
		}

		

		public Farm[] GetFarms(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Farm() { FarmID = t,BuildingID=PlanetID });
		}

		public void SetHealthPoints(int FarmID, int HealthPoints)
		{
			throw new NotImplementedException();
		}

		public Farm CreateFarm(int PlanetID, int X, int Y, int RemainingBuildSteps, BuildingTypeIDs BuildingTypeID, FarmTypeIDs FarmTypeID)
		{
			throw new NotImplementedException();
		}


	}
}
