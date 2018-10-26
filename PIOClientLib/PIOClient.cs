using LogLib;
using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOClientLib
{
	public abstract class PIOClient : Module, IPIOClient
	{
		public PIOClient(ILogger Logger) : base( Logger)
		{
		}


		protected abstract IEnumerable<Row> OnGetFactories(int PlanetID);
		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			return TryGetOrThrow(()=>OnGetFactories(PlanetID), (ex) => new PIOClientException("Failed to get factories",ex));
		}

		protected abstract Row OnGetPlanet(int PlanetID);
		public Row GetPlanet(int PlanetID)
		{
			return TryGetOrThrow(() => OnGetPlanet(PlanetID), (ex) => new PIOClientException("Failed to get planet", ex));
		}

		protected abstract IEnumerable<Row> OnGetPlanets();
		public IEnumerable<Row> GetPlanets()
		{
			return TryGetOrThrow(() => OnGetPlanets(), (ex) => new PIOClientException("Failed to get planets", ex));
		}
	}
}
