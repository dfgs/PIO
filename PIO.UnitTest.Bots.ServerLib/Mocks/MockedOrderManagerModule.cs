using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.Bots.ServiceLib.Mocks
{
	public class MockedOrderManagerModule : IOrderManagerModule
	{
		public int ProduceOrderCount
		{
			get;
			private set;
		}

		public int TaskCreated
		{
			get;
			private set;
		}

		private bool throwException;

		public MockedOrderManagerModule(bool ThrowException)
		{
			this.throwException = ThrowException;
		}

		public ProduceOrder CreateProduceOrder(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public Models.Task CreateTask(int WorkerID)
		{
			if (throwException) throw new Exception("RunTask ERROR");

			TaskCreated++;
			return new Models.Task() { WorkerID = WorkerID,ETA=DateTime.Now };
		}
	}
}
