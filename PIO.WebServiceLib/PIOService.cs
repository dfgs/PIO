using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PIO.Models;

namespace PIO.WebServiceLib
{
	public class PIOService : IPIOService
	{
		public Planet GetPlanet(int PlanetID)
		{
			return new Planet();
		}
	}
}
