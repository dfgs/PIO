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
	public class MockedBuildingTypeModule : MockedDatabaseModule<BuildingType>, IBuildingTypeModule
	{

		public MockedBuildingTypeModule(bool ThrowException, params BuildingType[] Items) : base(ThrowException,Items)
		{
		}

		public BuildingType CreateBuildingType(BuildingTypeIDs BuildingTypeID, string Name, int BuildSteps, int HealthPoints)
		{
			throw new NotImplementedException();
		}

		public BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.BuildingTypeID == BuildingTypeID);
		}

		public BuildingType[] GetBuildingTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.ToArray();
		}


	}
}
