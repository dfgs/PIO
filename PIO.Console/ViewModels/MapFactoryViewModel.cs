using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIO.Console.ViewModels
{
	public class MapFactoryViewModel:MapItemViewModel
	{
		public static readonly DependencyProperty StacksProperty = DependencyProperty.Register("Stacks", typeof(StacksViewModel), typeof(MapFactoryViewModel));
		public StacksViewModel Stacks
		{
			get { return (StacksViewModel)GetValue(StacksProperty); }
			set { SetValue(StacksProperty, value); }
		}

		public MapFactoryViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
		}

		protected override async System.Threading.Tasks.Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await Stacks.RefreshAsync();
		}

		protected override async Task<ILocation> OnLoadModelAsync()
		{
			return await PIOClient.GetFactoryAsync(((Factory)Model).FactoryID);

		}

		
		protected override async System.Threading.Tasks.Task OnLoadAsync(ILocation Model)
		{
			Stacks = new StacksViewModel(PIOClient, BotsClient, ((Factory)Model).BuildingID);
			await Stacks.LoadAsync();
		}


	}
}
