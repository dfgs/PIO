using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IFactoryModule:IModule
	{
		Row GetFactory(int FactoryID);
		IEnumerable<Row> GetFactories(int PlanetID);
	}
}
