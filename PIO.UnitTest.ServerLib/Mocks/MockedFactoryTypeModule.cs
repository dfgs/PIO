using PIO.Models;
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

		public MockedFactoryTypeModule(params FactoryType[] Items) : base(Items)
		{
		}

		public FactoryType GetFactoryType(int FactoryTypeID)
		{
			if (ThrowException) throw new InvalidOperationException();
			return items.FirstOrDefault(item => item.FactoryTypeID == FactoryTypeID);
		}

		public FactoryType[] GetFactoryTypes()
		{
			if (ThrowException) throw new InvalidOperationException();
			return items;
		}


	}
}
