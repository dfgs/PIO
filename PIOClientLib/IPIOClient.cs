using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

		Row<Planet> GetPlanet(int PlanetID);
		IEnumerable<Row<Planet>> GetPlanets();

		IEnumerable<Row<Factory>> GetFactories(int PlanetID);
		Row<Factory> BuildFactory(int PlanetID, int FactoryTypeID);

		IEnumerable<Row<Stack>> GetStacks(int FactoryID);
		Row<Task> GetTask(int FactoryID);

		Row<State> GetState(int StateID);
	}
}
