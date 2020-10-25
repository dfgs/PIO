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
		public Factory GetFactory(int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault();
		}

		/*public void SetHealthPoints(int FactoryID, int HealthPoints)
		{
			Factory factory;

			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");

			factory = GetFactory(FactoryID);
			if (factory == null) return;
			factory.HealthPoints+=HealthPoints;
		}*/


	}
}
