using LogLib;
using NetORMLib;
using PIOClientLib;
using PIOViewModelLib;
using PIOViewModelLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib
{
	public class PlanetViewModel : RowViewModel
	{
		public int PlanetID
		{
			get { return Model.PlanetID; }
		}
		public string Name
		{
			get { return Model.Name; }
		}

		public PlanetViewModel(ILogger Logger, IPIOClient Client) : base(Logger, Client)
		{
		}

		protected override Row OnLoad()
		{
			return Client.GetPlanet(0);
		}


	}
}
