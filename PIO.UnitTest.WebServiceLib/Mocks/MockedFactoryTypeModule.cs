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
	public class MockedFactoryTypeModule : MockedDatabaseModule, IFactoryTypeModule
	{
		public MockedFactoryTypeModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public FactoryType GetFactoryType(int FactoryTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new FactoryType() { FactoryTypeID = FactoryTypeID };
		}

		public FactoryType[] GetFactoryTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new FactoryType() { FactoryTypeID = t });
		}

		
	}
}
