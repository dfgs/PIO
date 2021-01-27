using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class FactoryViewModel : PIOViewModel<Factory>
	{


		public static readonly DependencyProperty StacksProperty = DependencyProperty.Register("Stacks", typeof(StacksViewModel), typeof(FactoryViewModel));
		public StacksViewModel Stacks
		{
			get { return (StacksViewModel)GetValue(StacksProperty); }
			set { SetValue(StacksProperty, value); }
		}


		public FactoryViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
		}

		protected override async Task OnRefreshAsync()
		{
			await base.OnRefreshAsync();
			await Stacks.RefreshAsync();
		}

		protected override async Task<Factory> OnLoadModelAsync()
		{
			return await PIOClient.GetFactoryAsync(Model.FactoryID);
		}
		protected override async Task OnLoadAsync(Factory Model)
		{
			Stacks = new StacksViewModel(PIOClient, BotsClient, Model.BuildingID);
			await Stacks.LoadAsync();
		}
		
	
	}
}
