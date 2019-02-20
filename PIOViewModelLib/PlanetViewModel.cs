using LogLib;
using NetORMLib;
using PIOClientLib;
using PIOServerLib.Rows;
using PIOServerLib.Tables;
using PIOViewModelLib;
using PIOViewModelLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib
{
	public class PlanetViewModel : RowViewModel<PlanetRow>
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

		public PlanetViewModel(ILogger Logger, IPIOClient Client) : base(Logger, Client)
		{
			Factories = new FactoriesViewModel(Logger,Client);
		}

		protected override void OnLoaded()
		{
			Factories.Load(() => Client.GetFactories(PlanetID));
		}

		public void BuildFactory()
		{
			FactoryRow item;
			LogEnter();

			try
			{
				item = Client.BuildFactory(PlanetID, 1);
				Factories.Add(item);
			}
			catch(Exception ex)
			{
				Log(ex);
			}
		}


	}
}
