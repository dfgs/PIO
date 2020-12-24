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
	public class MockedFactoryModule : MockedDatabaseModule, IFactoryModule
	{
		public MockedFactoryModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public Factory GetFactory(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Factory() { FactoryID = FactoryID };
		}
		public Factory GetFactory(int PlanetID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Factory() ;
		}

		

		public Factory[] GetFactories(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Factory() { FactoryID = t,BuildingID=PlanetID });
		}

		public void SetHealthPoints(int FactoryID, int HealthPoints)
		{
			throw new NotImplementedException();
		}

		public Factory CreateFactory(int PlanetID, int X, int Y, int RemainingBuildSteps, FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}


	}
}
