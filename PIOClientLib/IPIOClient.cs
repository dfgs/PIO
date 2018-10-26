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
		bool IsConnected
		{
			get;
		}

		void Connect();
		void Disconnect();

		Row GetPlanet(int PlanetID);
		IEnumerable<Row> GetPlanets();
		IEnumerable<Row> GetFactories(int PlanetID);

	}
}
