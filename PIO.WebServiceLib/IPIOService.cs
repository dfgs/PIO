using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PIO.WebServiceLib
{
	[ServiceContract]
	public interface IPIOService
	{

		[OperationContract]
		Planet GetPlanet(int PlanetID);

	}

	
}
