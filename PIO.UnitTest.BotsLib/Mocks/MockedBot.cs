using PIO.BotsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.BotsLib.Mocks
{
	public class MockedBot : IBot
	{
		public int WorkerID => 1;

		private bool isIdle;
		private bool throwExceptionOnGetCurrentTask;
		private bool throwExceptionOnRunTask;

		public int TaskCreated
		{
			get;
			private set;
		}

		public MockedBot(bool IsIdle, bool ThrowExceptionOnGetCurrentState, bool ThrowExceptionOnRunTask)
		{
			this.isIdle = IsIdle;this.throwExceptionOnGetCurrentTask = ThrowExceptionOnGetCurrentState;throwExceptionOnRunTask = ThrowExceptionOnRunTask;

		}
		public Models.Task GetCurrentTask()
		{
			if (throwExceptionOnGetCurrentTask) throw new InvalidOperationException("GetCurrentTask ERROR");
			if (isIdle) return null;
			return new Models.Task() { WorkerID = WorkerID,ETA=DateTime.Now.AddSeconds(1) };
		}

		public Models.Task RunTask()
		{
			if (throwExceptionOnRunTask) throw new InvalidOperationException("RunTask ERROR");
			TaskCreated++;
			return new Models.Task() { WorkerID = WorkerID, ETA = DateTime.Now.AddSeconds(1) };
		}

	}
}
