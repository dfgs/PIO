using LogLib;
using NetORMLib;
using PIOClientLib;
using PIOServerLib.Rows;
using PIOViewModelLib;
using PIOViewModelLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib
{
	public class PlanetsViewModel : RowsViewModel<PlanetViewModel,PlanetRow>
	{
		public PlanetsViewModel(ILogger Logger, IPIOClient Client) : base(Logger,Client, ()=>new PlanetViewModel(Logger,Client))
		{
		}
	

	}
}
