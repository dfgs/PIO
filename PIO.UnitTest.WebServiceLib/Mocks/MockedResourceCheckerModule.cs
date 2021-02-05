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
	public class MockedResourceCheckerModule :MockedFunctionalModule,  IResourceCheckerModule
	{

		private bool result;

		public MockedResourceCheckerModule(bool ThrowException,bool Result) : base( ThrowException)
		{
			this.result = Result;
		}

		public ResourceTypeIDs[] GetMissingResourcesToProduce(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			if (result) return new ResourceTypeIDs[] { ResourceTypeIDs.Coal};
			else return new ResourceTypeIDs[] { };
		}

		public bool HasEnoughResourcesToProduce(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return result;
		}

		public ResourceTypeIDs[] GetMissingResourcesToBuild(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			if (result) return new ResourceTypeIDs[] { ResourceTypeIDs.Coal };
			else return new ResourceTypeIDs[] { };
		}

		public bool HasEnoughResourcesToBuild(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return result;
		}
	}
}
