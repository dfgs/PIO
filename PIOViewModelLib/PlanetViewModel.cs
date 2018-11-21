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

		public FactoriesViewModel Factories
		{
			get;
			private set;
		}

		public PlanetViewModel(ILogger Logger) : base(Logger)
		{
			Factories = new FactoriesViewModel(Logger);
		}

		protected override void OnLoaded(IPIOClient Client)
		{
			Factories.Load(Client,Client.GetFactories(PlanetID));
		}


	}
}
