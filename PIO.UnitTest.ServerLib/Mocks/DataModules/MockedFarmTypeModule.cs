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
	public class MockedFarmTypeModule : MockedDatabaseModule<FarmType>, IFarmTypeModule
	{

		public MockedFarmTypeModule(bool ThrowException, params FarmType[] Items) : base(ThrowException,Items)
		{
		}

		public FarmType GetFarmType(FarmTypeIDs FarmTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.FarmTypeID == FarmTypeID);
		}

		public FarmType[] GetFarmTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.ToArray();
		}


	}
}
