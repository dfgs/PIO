using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class TasksViewModel : PIOViewModelCollection<TaskViewModel,PIO.Models.Task>
	{
		private int workerID;
		public TasksViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,int WorkerID) : base(PIOClient, BotsClient)
		{
			this.workerID = WorkerID;
		}

		protected override TaskViewModel OnCreateItem()
		{
			return new TaskViewModel(PIOClient, BotsClient);
		}

		protected override async Task<IEnumerable<PIO.Models.Task>> OnLoadModelAsync()
		{
			return await PIOClient.GetTasksAsync(workerID);
		}
		

	}
}
