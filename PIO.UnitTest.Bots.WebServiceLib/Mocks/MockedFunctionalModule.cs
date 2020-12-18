using PIO.Bots.Models.Modules;
using PIO.ModulesLib.Modules.FunctionalModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.Bots.WebServiceLib.Mocks
{
	public abstract class MockedFunctionalModule:IFunctionalModule
	{
		

		private bool throwException;
		public bool ThrowException
		{
			get { return throwException; }
		}
		public MockedFunctionalModule(bool ThrowException)
		{
			this.throwException = ThrowException;
		}
		
		

	}

}
