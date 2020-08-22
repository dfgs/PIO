using PIO.Models;
using PIO.Models.Modules;
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
			if (ThrowException) throw new Exception("Mocked exception");
			return new Factory() { FactoryID = FactoryID };
		}

		/*public Factory[] GetFactories()
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Factory() { FactoryID = t });
		}*/

		public Factory[] GetFactories(int PlanetID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Factory() { FactoryID = t,PlanetID=PlanetID });
		}

		public void SetHealthPoints(int FactoryID, int HealthPoints)
		{
			throw new NotImplementedException();
		}
	}
}
