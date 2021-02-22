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
	public class MockedProductModule :IProductModule
	{
		private bool throwException;
		private List<Product> items;

		public MockedProductModule(bool ThrowException, params Product[] Items)
		{
			this.throwException = ThrowException;
			this.items = new List<Product>(Items);
		}

		public Product CreateProduct(BuildingTypeIDs BuildingTypeID, ResourceTypeIDs ResourceTypeID, int Quantity, int Duration)
		{
			throw new NotImplementedException();
		}

		public Product GetProduct(int ProductID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.ProductID == ProductID);
		}

		public Product[] GetProducts(BuildingTypeIDs BuildingTypeID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.BuildingTypeID == BuildingTypeID).ToArray();
		}



		
	}
}
