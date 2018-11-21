using NetORMLib;
using PIOServerLib.Modules;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOUnitTest.Mocks
{
	public class MockedFactoryModule : MockedModule<Factory>, IFactoryModule
	{

		public MockedFactoryModule(bool ThrowException):base(ThrowException)
		{
		}

		public Row GetFactory(int FactoryID)
		{
			return GenerateRows(1).FirstOrDefault();
		}

		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			return GenerateRows(3,(item)=>item.PlanetID=PlanetID);
		}

		public Row BuildFactory(int PlanetID, int FactoryTypeID)
		{
			return GenerateRows(1, (item) => item.PlanetID = PlanetID).First();
		}

	}
}
