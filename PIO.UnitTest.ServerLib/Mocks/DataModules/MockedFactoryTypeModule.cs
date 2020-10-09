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
	public class MockedFactoryTypeModule : MockedDatabaseModule<FactoryType>, IFactoryTypeModule
	{

		public MockedFactoryTypeModule(bool ThrowException, params FactoryType[] Items) : base(ThrowException,Items)
		{
		}

		public FactoryType GetFactoryType(FactoryTypeIDs FactoryTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.FactoryTypeID == FactoryTypeID);
		}

		public FactoryType[] GetFactoryTypes()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.ToArray();
		}


	}
}
