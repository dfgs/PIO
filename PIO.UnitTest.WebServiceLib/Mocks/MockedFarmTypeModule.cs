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
	public class MockedFarmTypeModule : MockedDatabaseModule, IFarmTypeModule
	{
		public MockedFarmTypeModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public FarmType GetFarmType(FarmTypeIDs FarmTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new FarmType() { FarmTypeID = FarmTypeID };
		}

		public FarmType[] GetFarmTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new FarmType() { FarmTypeID = (FarmTypeIDs)t });
		}

		
	}
}
