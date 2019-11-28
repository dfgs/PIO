using ModuleLib;
using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IFactoryModule:IDatabaseModule
	{
		Row<Factory> GetFactory(int FactoryID);
		IEnumerable<Row<Factory>> GetFactories(int PlanetID);
		int CreateFactory(int PlanetID,int FactoryTypeID,int StateID);
		void SetState(int FactoryID, int StateID);
	}
}
