using NetORMLib;
using PIOServerLib.Modules;
using PIOServerLib.Rows;
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

		public FactoryRow GetFactory(int FactoryID)
		{
			return GenerateRows<FactoryRow>(1,(item)=>item.Name= "New factory").FirstOrDefault();
		}

		public IEnumerable<FactoryRow> GetFactories(int PlanetID)
		{
			return GenerateRows<FactoryRow>(3, (item) => {item.PlanetID = PlanetID; item.Name = "New factory"; });
		}

		public FactoryRow BuildFactory(int PlanetID, int FactoryTypeID)
		{
			return GenerateRows<FactoryRow>(1, (item) => { item.PlanetID = PlanetID; item.Name = "New factory"; }).First();
		}

		public int CreateFactory(int PlanetID, int FactoryTypeID,int StateID)
		{
			throw new NotImplementedException();
		}

		public void SetState(int FactoryID, int StateID)
		{
			throw new NotImplementedException();
		}
	}
}
