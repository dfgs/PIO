using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.ClientLib.TaskCallbackServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace PIO.Console.ViewModels
{
	public class ApplicationViewModel:PIOViewModel<int>
	{

		public static readonly DependencyProperty WorkersProperty = DependencyProperty.Register("Workers", typeof(WorkersViewModel), typeof(ApplicationViewModel));
		public WorkersViewModel Workers
		{
			get { return (WorkersViewModel)GetValue(WorkersProperty); }
			set { SetValue(WorkersProperty, value); }
		}


		public static readonly DependencyProperty FactoriesProperty = DependencyProperty.Register("Factories", typeof(FactoriesViewModel), typeof(ApplicationViewModel));
		public FactoriesViewModel Factories
		{
			get { return (FactoriesViewModel)GetValue(FactoriesProperty); }
			set { SetValue(FactoriesProperty, value); }
		}

		public ApplicationViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{
			Workers = new WorkersViewModel(PIOClient,BotsClient,1);
			Factories = new FactoriesViewModel(PIOClient, BotsClient, 1);
	
		}

		protected override async Task<int> OnLoadModelAsync()
		{
			await Task.Delay(0);
			return 0;
		}

		public override async Task LoadAsync(int Model)
		{
			await Workers.LoadAsync();
			await Factories.LoadAsync();
		}


		



	}
}
