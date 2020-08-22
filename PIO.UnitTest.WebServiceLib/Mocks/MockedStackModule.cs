using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedStackModule : MockedDatabaseModule, IStackModule
	{
		public MockedStackModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public void Consume(int FactoryID, int ResourceTypeID, int Quantity)
		{
			throw new NotImplementedException();
		}

		public Stack GetStack(int StackID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return new Stack() { StackID = StackID };
		}

		public Stack[] GetStacks(int FactoryID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Stack() { StackID = t,FactoryID=FactoryID });
		}

		public bool HasEnoughResources(int FactoryID, int ResourceTypeID, int Quantity)
		{
			throw new NotImplementedException();
		}
	}
}
