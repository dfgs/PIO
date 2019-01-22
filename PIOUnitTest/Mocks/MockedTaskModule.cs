using NetORMLib;
using PIOServerLib.Modules;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOUnitTest.Mocks
{
	public class MockedTaskModule : MockedModule<Task>, ITaskModule
	{

		public MockedTaskModule(bool ThrowException):base(ThrowException)
		{
		}

		public Row GetTask(int FactoryID)
		{
			return GenerateRows(1).FirstOrDefault();
		}

		public void SetTask(int FactoryID, int TaskID)
		{
			throw new NotImplementedException();
		}
	}
}
