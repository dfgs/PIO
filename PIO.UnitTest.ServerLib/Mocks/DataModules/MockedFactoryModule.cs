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
	public class MockedFactoryModule :MockedDatabaseModule<Factory>, IFactoryModule
	{

		public MockedFactoryModule(bool ThrowException, params Factory[] Items):base(ThrowException,Items)
		{
		}

		

		public Factory[] GetFactories(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.BuildingID == PlanetID).ToArray();
		}

		public Factory GetFactory(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.FactoryID == FactoryID);
		}
		public Factory GetFactory(int PlanetID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			if (X != 0) return null;
			return items.FirstOrDefault();
		}

		public Factory CreateFactory(int BuildingID, FactoryTypeIDs FactoryTypeID)
		{
			Factory result;

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			result = new Factory() { FactoryID = items.Count, BuildingID = BuildingID };
			items.Add(result);
			return result;
		}


	}
}
