using NetORMLib;
using PIOServerLib;
using PIOServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOUnitTest.Mocks
{
	public class MockedPIOServer : IPIOServer
	{
		public IPlanetModule PlanetModule
		{
			get;
			private set;
		}
		public IFactoryModule FactoryModule
		{
			get;
			private set;
		}
		public IStackModule StackModule
		{
			get;
			private set;
		}

		public bool IsInitialized
		{
			get;
			private set;
		}

		public MockedPIOServer(bool IsInitialized,bool ThrowException)
		{
			this.IsInitialized = IsInitialized;
			this.PlanetModule = new MockedPlanetModule(ThrowException);
			this.FactoryModule = new MockedFactoryModule(ThrowException);
			this.StackModule = new MockedStackModule(ThrowException);
		}

		public Row BuildFactory(int PlanetID, int FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Row GetPlanet(int PlanetID)
		{
			return PlanetModule.GetPlanet(PlanetID);
		}
		public IEnumerable<Row> GetPlanets()
		{
			return PlanetModule.GetPlanets();
		}

		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			return FactoryModule.GetFactories(PlanetID);
		}

		public IEnumerable<Row> GetStacks(int FactoryID)
		{
			return StackModule.GetStacks(FactoryID);
		}


	}
}
