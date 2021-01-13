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
	public class WorkerViewModel : PIOViewModel<Worker>
	{

		public static readonly DependencyProperty WorkerIDProperty = DependencyProperty.Register("WorkerID", typeof(int), typeof(WorkerViewModel));
		public int WorkerID
		{
			get { return (int)GetValue(WorkerIDProperty); }
			set { SetValue(WorkerIDProperty, value); }
		}

		public WorkerViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) : base(PIOClient, BotsClient)
		{

		}

		public override async Task LoadAsync(Worker Model)
		{
			this.WorkerID = Model.WorkerID;
			await Task.Delay(0);
		}

		protected override async Task<Worker> OnLoadModelAsync()
		{
			return await PIOClient.GetWorkerAsync(WorkerID);
		}

	}
}
