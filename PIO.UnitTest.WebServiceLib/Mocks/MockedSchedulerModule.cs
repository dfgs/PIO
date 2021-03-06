﻿using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedSchedulerModule : ISchedulerModule
	{
		private bool throwException;
		
		public event TaskEventHandler TaskStarted;
		public event TaskEventHandler TaskEnded;


		public MockedSchedulerModule(bool ThrowException)
		{
			this.throwException = ThrowException;
		}

		public void InvokeTaskStarted()
		{
			if (TaskStarted != null) TaskStarted(this, new TaskEventArgs(null));
		}
		public void InvokeTaskEnded()
		{
			if (TaskEnded != null) TaskEnded(this, new TaskEventArgs(null));
		}

	}
}
