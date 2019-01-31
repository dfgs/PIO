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
		public ITaskModule TaskModule
		{
			get;
			private set;
		}
		public IStateModule StateModule
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
			this.TaskModule = new MockedTaskModule(ThrowException);
			this.StateModule = new MockedStateModule(ThrowException);
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

		public IEnumerable<Row> GetTasks(int FactoryID)
		{
			return TaskModule.GetTasks(FactoryID);
		}

		public Row GetState(int StateID)
		{
			return StateModule.GetState(StateID);
		}

		public IEnumerable<Row> GetStacks(int FactoryID)
		{
			return StackModule.GetStacks(FactoryID);
		}


	}
}
