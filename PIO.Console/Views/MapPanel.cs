using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PIO.Console.Views
{
	
	public class MapPanel : Panel
	{


		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(int), typeof(MapPanel), new FrameworkPropertyMetadata(16,FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
		public int ItemWidth
		{
			get { return (int)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(int), typeof(MapPanel), new FrameworkPropertyMetadata(16, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
		public int ItemHeight
		{
			get { return (int)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}


		public static readonly DependencyProperty XProperty = DependencyProperty.RegisterAttached("X", typeof(int), typeof(MapPanel), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentArrange));
		public static readonly DependencyProperty YProperty = DependencyProperty.RegisterAttached("Y", typeof(int), typeof(MapPanel), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentArrange));


		public static readonly RoutedEvent MapLeftClickedEvent = EventManager.RegisterRoutedEvent("MapLeftClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MapPanel));
		public event RoutedEventHandler MapLeftClicked
		{
			add { AddHandler(MapLeftClickedEvent, value); }
			remove { RemoveHandler(MapLeftClickedEvent, value); }
		}

		public static readonly RoutedEvent MapRightClickedEvent = EventManager.RegisterRoutedEvent("MapRightClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MapPanel));
		public event RoutedEventHandler MapRightClicked
		{
			add { AddHandler(MapRightClickedEvent, value); }
			remove { RemoveHandler(MapRightClickedEvent, value); }
		}




		public static int GetX(UIElement obj)
		{
			return (int)obj.GetValue(XProperty);
		}

		public static void SetX(UIElement obj, int value)
		{
			obj.SetValue(XProperty, value);
		}

		public static int GetY(UIElement obj)
		{
			return (int)obj.GetValue(YProperty);
		}

		public static void SetY(UIElement obj, int value)
		{
			obj.SetValue(YProperty, value);
		}



		protected override Size MeasureOverride(Size availableSize)
		{
			int x, y;
			float currentX, currentY;
			float maxX, maxY;
			Size itemSize;

			maxX = 0;maxY = 0;
			itemSize = new Size(ItemWidth, ItemHeight);
			foreach (UIElement element in this.Children)
			{
				x = GetX(element);
				y = GetY(element);

				element.Measure(itemSize);
				
				currentX = (x + 1) * ItemWidth;
				currentY = (y + 1) * ItemHeight;

				if (currentX > maxX) maxX = currentX;
				if (currentY > maxY) maxY = currentY;
			}

			return new Size(maxX, maxY);

		}
		protected override Size ArrangeOverride(Size finalSize)
		{
			int x, y;
			float currentX, currentY;
			float maxX, maxY;

			maxX = 0; maxY = 0;
			foreach (UIElement element in this.Children)
			{
				x = GetX(element);
				y = GetY(element);

				element.Arrange(new Rect(x*ItemWidth,y*ItemHeight,ItemWidth,ItemHeight ));

				currentX = (x + 1) * ItemWidth;
				currentY = (y + 1) * ItemHeight;

				if (currentX > maxX) maxX = currentX;
				if (currentY > maxY) maxY = currentY;
			}

			return new Size(maxX, maxY);
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			Point pos;
			int x, y;

			base.OnMouseLeftButtonUp(e);

			pos = e.GetPosition(this);
			x = (int)(pos.X / ItemWidth);
			y= (int)(pos.Y/ItemHeight);
			
			RoutedEventArgs args = new MapClickedRoutedEventArgs(MapPanel.MapLeftClickedEvent, x, y);
			RaiseEvent(args);
		}

		protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
		{
			Point pos;
			int x, y;

			base.OnMouseRightButtonUp(e);

			pos = e.GetPosition(this);
			x = (int)(pos.X / ItemWidth);
			y = (int)(pos.Y / ItemHeight);

			RoutedEventArgs args = new MapClickedRoutedEventArgs(MapPanel.MapRightClickedEvent, x, y);
			RaiseEvent(args);
		}


	}
}
