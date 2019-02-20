using ModuleLib;
using NetORMLib;
using PIOServerLib.Rows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IFactoryModule:IDatabaseModule
	{
		FactoryRow GetFactory(int FactoryID);
		IEnumerable<FactoryRow> GetFactories(int PlanetID);
		int CreateFactory(int PlanetID,int FactoryTypeID,int StateID);
		void SetState(int FactoryID, int StateID);
	}
}
