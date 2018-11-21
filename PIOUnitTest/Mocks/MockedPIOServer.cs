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




	}
}
