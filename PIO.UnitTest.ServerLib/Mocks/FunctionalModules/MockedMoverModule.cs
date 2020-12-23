using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedMoverModule : IMoverModule
	{
		public event TaskCreatedHandler TaskCreated;

		public Models.Task BeginMoveTo(int WorkerID, int X, int Y)
		{
			Task task = new Models.Task() { WorkerID = WorkerID };
			TaskCreated(this, task);
			return task;
		}
		public Models.Task BeginMoveTo(int WorkerID, int FactoryID)
		{
			Task task = new Models.Task() { WorkerID = WorkerID };
			TaskCreated(this, task);
			return task;
		}

		public void EndMoveTo(int WorkerID,int X,int Y)
		{
		}
	}
}
