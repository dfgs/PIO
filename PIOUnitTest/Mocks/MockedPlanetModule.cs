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
	public class MockedPlanetModule : MockedModule<Planet>, IPlanetModule
	{

		public MockedPlanetModule(bool ThrowException):base(ThrowException)
		{
		}
		
		public IEnumerable<Row> GetPlanets()
		{
			return GenerateRows(1);
		}

	}
}
