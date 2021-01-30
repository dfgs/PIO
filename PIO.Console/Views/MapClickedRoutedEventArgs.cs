using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIO.Console.Views
{
	//public delegate void MapClickedEventHandler(object sender, MapClickedEventArgs e);
	public class MapClickedRoutedEventArgs: RoutedEventArgs
	{
		public int X
		{
			get;
			private set;
		}
		public int Y
		{
			get;
			private set;
		}

		public MapClickedRoutedEventArgs(RoutedEvent Event,int X,int Y):base(Event)
		{
			this.X = X;this.Y = Y;
		}

	}


}
