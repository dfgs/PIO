using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IFactoryModule:IDatabaseModule
	{
		Row GetFactory(int FactoryID);
		IEnumerable<Row> GetFactories(int PlanetID);
		int CreateFactory(int PlanetID,int FactoryTypeID,int StateID);
		void SetState(int FactoryID, int StateID);
	}
}
