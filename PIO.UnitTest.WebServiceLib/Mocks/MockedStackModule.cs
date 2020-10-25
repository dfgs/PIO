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
	public class MockedStackModule : MockedDatabaseModule, IStackModule
	{
		public MockedStackModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public Stack GetStack(int StackID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Stack() { StackID = StackID };
		}

		public int GetStackQuantity(int FactoryID, ResourceTypeIDs ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return 3;
		}
		public Stack GetStack(int FactoryID, ResourceTypeIDs ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Stack() { StackID = 1,ResourceTypeID=ResourceTypeID };
		}
		public Stack[] GetStacks(int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Stack() { StackID = t,FactoryID=FactoryID });
		}

		public Stack InsertStack(int FactoryID, ResourceTypeIDs ResourceTypeID, int Quantity)
		{
			throw new NotImplementedException();
		}

		public void UpdateStack(int StackID, int Quantity)
		{
			throw new NotImplementedException();
		}
	}
}
