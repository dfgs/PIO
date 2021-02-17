using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedFarmModule :MockedDatabaseModule<Farm>, IFarmModule
	{

		public MockedFarmModule(bool ThrowException, params Farm[] Items):base(ThrowException,Items)
		{
		}

		

		public Farm[] GetFarms(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.BuildingID == PlanetID).ToArray();
		}

		public Farm GetFarm(int FarmID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.FarmID == FarmID);
		}
		public Farm GetFarm(int PlanetID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			if (X != 0) return null;
			return items.FirstOrDefault();
		}

		public Farm CreateFarm(int PlanetID, int X, int Y, int RemainingBuildSteps, FarmTypeIDs FarmTypeID)
		{
			Farm result;

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			result=new Farm() { FarmID = items.Count, BuildingID = 1,PlanetID=PlanetID,X=X,Y=Y,RemainingBuildSteps=RemainingBuildSteps,FarmTypeID=FarmTypeID };
			items.Add(result);
			return result;
		}


	}
}
