using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace PIO.Console.ViewModels
{
	public class CellViewModel : PIOViewModel<Cell>,ILocationViewModel
	{
		public int X
		{
			get { return Model.X; }
		}
		public int Y
		{
			get { return Model.Y; }
		}
		
		public string Description
		{
			get { return $"Cell {X},{Y}"; }
		}

		/*public static readonly DependencyProperty MoveToCommandProperty = DependencyProperty.Register("MoveToCommand", typeof(ViewModelCommand), typeof(CellViewModel), new PropertyMetadata(null));
		public ViewModelCommand MoveToCommand
		{
			get { return (ViewModelCommand)GetValue(MoveToCommandProperty); }
			set { SetValue(MoveToCommandProperty, value); }

		}*/
		public CellViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			//MoveToCommand = new ViewModelCommand(MoveToCommandCanExecute, MoveToCommandExecute);

		}
		/*private bool MoveToCommandCanExecute(object arg)
		{
			return (Model != null) ;
		}
		private async void MoveToCommandExecute(object obj)
		{
			await System.Threading.Tasks.Task.Yield();
		}*/

		protected override async Task<Cell> OnLoadModelAsync()
		{
			return await PIOClient.GetCellAsync(((Cell)Model).CellID);
		}
	}
}
