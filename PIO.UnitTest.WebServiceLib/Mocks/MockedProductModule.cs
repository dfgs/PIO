using PIO.Models;
using PIO.Models.Modules;
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

		

		public Product GetProduct(int ProductID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return new Product() { ProductID = ProductID };
		}

		public Product[] GetProducts(int FactoryTypeID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Product() { ProductID = t,FactoryTypeID=FactoryTypeID });
		}

		
	}
}
