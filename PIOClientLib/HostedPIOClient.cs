using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;
using NetORMLib;
using PIOServerLib;
using PIOServerLib.Modules;

namespace PIOClientLib
{
	public class HostedPIOClient : PIOClient
	{
		private IPlanetModule planetModule;

		public HostedPIOClient(ILogger Logger, IPlanetModule PlanetModule):base("HostedPIOClient",Logger)
		{
			this.planetModule = PlanetModule;
		}

		protected override IEnumerable<Row> OnGetFactories(int PlanetID)
		{
			throw new NotImplementedException();
		}
		protected override IEnumerable<Row> OnGetPlanets()
		{
			return planetModule.GetPlanets();
		}



	}
}
