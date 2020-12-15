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
	public class MockedBuildingTypeModule : MockedDatabaseModule, IBuildingTypeModule
	{
		public MockedBuildingTypeModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new BuildingType() { BuildingTypeID = BuildingTypeID };
		}

		public BuildingType[] GetBuildingTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new BuildingType() { BuildingTypeID = (BuildingTypeIDs)t });
		}

		
	}
}
