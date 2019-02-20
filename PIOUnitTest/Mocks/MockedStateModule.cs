using NetORMLib;
using PIOServerLib.Modules;
using PIOServerLib.Rows;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOUnitTest.Mocks
{
	public class MockedStateModule : MockedModule<State>, IStateModule
	{

		public MockedStateModule(bool ThrowException):base(ThrowException)
		{
		}

		public StateRow GetState(int StateID)
		{
			return GenerateRows<StateRow>(1,(item)=>item.Name="MockedState").FirstOrDefault();
		}

		
	}
}
