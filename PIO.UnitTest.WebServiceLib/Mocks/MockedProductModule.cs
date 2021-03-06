﻿using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedProductModule : MockedDatabaseModule, IProductModule
	{
		public MockedProductModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public Product CreateProduct(BuildingTypeIDs BuildingTypeID, ResourceTypeIDs ResourceTypeID, int Quantity, int Duration)
		{
			throw new NotImplementedException();
		}

		public Product GetProduct(int ProductID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Product() { ProductID = ProductID };
		}

		public Product[] GetProducts(BuildingTypeIDs BuildingTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Product() { ProductID = t, BuildingTypeID = BuildingTypeID });
		}

		
	}
}
