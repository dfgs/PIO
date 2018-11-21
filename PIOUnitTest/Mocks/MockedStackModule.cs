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
	public class MockedStackModule : MockedModule<Stack>, IStackModule
	{

		public MockedStackModule(bool ThrowException):base(ThrowException)
		{
		}

		public Row GetStack(int StackID)
		{
			return GenerateRows(1).FirstOrDefault();
		}

		public IEnumerable<Row> GetStacks(int FactoryID)
		{
			return GenerateRows(3,(item)=>item.FactoryID=FactoryID);
		}

	}
}
