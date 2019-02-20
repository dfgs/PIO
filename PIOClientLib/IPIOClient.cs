using NetORMLib;
using PIOServerLib.Rows;
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

		PlanetRow GetPlanet(int PlanetID);
		IEnumerable<PlanetRow> GetPlanets();

		IEnumerable<FactoryRow> GetFactories(int PlanetID);
		FactoryRow BuildFactory(int PlanetID, int FactoryTypeID);

		IEnumerable<StackRow> GetStacks(int FactoryID);
		TaskRow GetTask(int FactoryID);

		StateRow GetState(int StateID);
	}
}
