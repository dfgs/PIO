using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PIO.Models;
using PIO.WebServerLib.Modules;

namespace PIO.WebServiceLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class PIOService : IPIOService
	{
		private IPlanetModule PlanetModule;
		private IFactoryModule FactoryModule;

		public PIOService(IPlanetModule PlanetModule, IFactoryModule FactoryModule)
		{
			this.PlanetModule = PlanetModule;
			this.FactoryModule = FactoryModule;
		}

		public Planet GetPlanet(int PlanetID)
		{
			return PlanetModule.GetPlanet(PlanetID);
		}
		public Planet[] GetPlanets()
		{
			return PlanetModule.GetPlanets().ToArray();
		}

		public Factory GetFactory(int FactoryID)
		{
			return FactoryModule.GetFactory(FactoryID);
		}
		public Factory[] GetFactories(int PlanetID)
		{
			return FactoryModule.GetFactories(PlanetID).ToArray();
		}

	}
}
