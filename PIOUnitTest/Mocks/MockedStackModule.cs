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
	public class MockedStackModule : MockedModule<Stack>, IStackModule
	{

		public MockedStackModule(bool ThrowException):base(ThrowException)
		{
		}

		public StackRow GetStack(int StackID)
		{
			return GenerateRows<StackRow>(1).FirstOrDefault();
		}

		public StackRow GetStack(int FactoryID, int ResourceID)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<StackRow> GetStacks(int FactoryID)
		{
			return GenerateRows<StackRow>(3,(item)=>item.FactoryID=FactoryID);
		}

	}
}
