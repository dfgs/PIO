using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedServiceCallback : ITaskCallBack
	{
		public System.Threading.Tasks.Task OnTaskEnded(Models.Task Task)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task OnTaskStarted(Models.Task Task)
		{
			throw new NotImplementedException();
		}
	}
}
