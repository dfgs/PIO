
using PIO.Models;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
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

		public event TaskEventHandler TaskStarted;
		public event TaskEventHandler TaskEnded;

		public int Count
		{
			get;
			set;
		}
		public MockedSchedulerModule(bool ThrowException,ITaskGeneratorModule TaskGeneratorModule)
		{
			this.ThrowException = ThrowException;
			TaskGeneratorModule.TaskCreated += ProducerModule_TaskCreated;
		}


		private void ProducerModule_TaskCreated(ITaskGeneratorModule Module, Models.Task Task)
		{
			Count++;
		}

		public void Add(Models.Task Task)
		{
			if (ThrowException) throw new PIOInternalErrorException("UnitTestException", null, 1, "UnitTest", "UnitTest");
		}

		

		
	}
}
