using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.Mocks
{
	class Test
	{
		public Test()
		{
			MockedWorkerModule test;

			test = new MockedWorkerModule();
			test.GetWorkerDelegate = (int WorkerID) => { return null; };


		}
	}
}
