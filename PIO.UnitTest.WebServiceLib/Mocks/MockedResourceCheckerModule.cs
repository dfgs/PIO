using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedResourceCheckerModule :MockedFunctionalModule,  IResourceCheckerModule
	{

		private bool? result;

		public MockedResourceCheckerModule(bool ThrowException,bool? Result) : base( ThrowException)
		{
			this.result = Result;
		}

		public bool? HasEnoughResourcesToProduce(int FactoryID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return result;
		}

	}
}
