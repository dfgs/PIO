using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedMaterialModule :IMaterialModule
	{
		private bool throwException;
		private List<Material> items;

		public MockedMaterialModule(bool ThrowException, params Material[] Items)
		{
			this.throwException = ThrowException;
			this.items = new List<Material>(Items);
		}

		public Material GetMaterial(int MaterialID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.MaterialID == MaterialID);
		}

		public Material[] GetMaterials(int MaterialSetID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.MaterialSetID == MaterialSetID).ToArray();
		}



		
	}
}
