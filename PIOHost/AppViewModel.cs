using LogLib;
using PIOClientLib;
using PIOViewModelLib;
using PIOViewModelLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIOHost
{
	public class AppViewModel:DependencyObject
	{
		public PlanetsViewModel Planets
		{
			get;
			private set;
		}

		private IPIOClient client;

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AppViewModel));
		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}


		public AppViewModel(ILogger Logger, IPIOClient Client)
		{
			this.client = Client;
			Planets = new PlanetsViewModel(Logger,Client);
		}


		public void Load()
		{
			Planets.Load(() => client.GetPlanets());
		}


	}
}
