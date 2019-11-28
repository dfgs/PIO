using NetORMLib;
using PIOServerLib;
using PIOServerLib.Modules;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

		public Row<Factory> BuildFactory(int PlanetID, int FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Row<Planet> GetPlanet(int PlanetID)
		{
			return PlanetModule.GetPlanet(PlanetID);
		}
		public IEnumerable<Row<Planet>> GetPlanets()
		{
			return PlanetModule.GetPlanets();
		}

		public IEnumerable<Row<Factory>> GetFactories(int PlanetID)
		{
			return FactoryModule.GetFactories(PlanetID);
		}

		public Row<Task> GetTask(int FactoryID)
		{
			return TaskModule.GetTask(FactoryID);
		}

		public Row<State> GetState(int StateID)
		{
			return StateModule.GetState(StateID);
		}

		public IEnumerable<Row<Stack>> GetStacks(int FactoryID)
		{
			return StackModule.GetStacks(FactoryID);
		}


	}
}
