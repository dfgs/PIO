using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOClientLib
{
    public interface IPIOClient
    {
		IEnumerable<Row> GetPlanets();
		IEnumerable<Row> GetFactories(int PlanetID);

	}
}
