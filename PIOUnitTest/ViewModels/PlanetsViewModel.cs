using LogLib;
using NetORMLib;
using PIOClientLib;
using PIOViewModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOUnitTest.ViewModels
{
	public class PlanetsViewModel : RowsViewModel<PlanetViewModel>
	{
		public PlanetsViewModel(ILogger Logger, IPIOClient Client) : base(Logger, Client, (logger,client)=>new PlanetViewModel(logger,client))
		{
		}
		protected override IEnumerable<Row> OnLoad()
		{
			return Client.GetPlanets();
		}
	}
}
