using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedProducerModule : IProducerModule
	{
		public event TaskCreatedHandler TaskCreated;

		public Models.Task BeginProduce(int WorkerID)
		{
			Task task=new Models.Task() { WorkerID = WorkerID };
			TaskCreated(this, new Task[] { task });
			return task;
		}

		public void EndProduce(int WorkerID)
		{
		}
	}
}
