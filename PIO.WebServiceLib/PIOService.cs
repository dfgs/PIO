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

		public PIOService(IPlanetModule PlanetModule)
		{
			this.PlanetModule = PlanetModule;
		}

		public Planet GetPlanet(int PlanetID)
		{
			//return new Planet() { PlanetID = PlanetID };
			return PlanetModule.GetPlanet(PlanetID);
		}
	}
}
