using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedCallback : ITaskCallBack
	{
		public int StartedCount;
		public int EndedCount;
		private bool throwException;

		public MockedCallback(bool ThrowException)
		{
			this.throwException = ThrowException;
		}

		public System.Threading.Tasks.Task OnTaskStarted(Models.Task Task)
		{
			if (throwException) throw new InvalidOperationException();
			StartedCount++;
			return System.Threading.Tasks.Task.Delay(0);
		}

		public System.Threading.Tasks.Task OnTaskEnded(Models.Task Task)
		{
			if (throwException) throw new InvalidOperationException();
			EndedCount++;
			return System.Threading.Tasks.Task.Delay(0);
		}

	}
}
