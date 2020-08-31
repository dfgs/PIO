using PIO.Models.Exceptions;
using PIO.ServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks.EngineModules
{
	public class MockedSchedulerModule : ISchedulerModule
	{
		public bool ThrowException
		{
			get;
			set;
		}
		
		public MockedSchedulerModule(bool ThrowException)
		{
			this.ThrowException = ThrowException;
		}

		public void Add(Models.Task Task)
		{
			if (ThrowException) throw new PIOInternalErrorException("UnitTestException", null, 1, "UnitTest", "UnitTest");
		}
	}
}
