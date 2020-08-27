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
	public class MockedMaterialModule : MockedDatabaseModule, IMaterialModule
	{
		public MockedMaterialModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public Material GetMaterial(int MaterialID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Material() { MaterialID = MaterialID };
		}

		public Material[] GetMaterials(int FactoryTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Material() { MaterialID = t,FactoryTypeID=FactoryTypeID });
		}

		
	}
}
