using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedResourceTypeModule : MockedDatabaseModule, IResourceTypeModule
	{
		public MockedResourceTypeModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public ResourceType GetResourceType(int ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new ResourceType() { ResourceTypeID = ResourceTypeID };
		}

		public ResourceType[] GetResourceTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new ResourceType() { ResourceTypeID = t });
		}

		
	}
}
