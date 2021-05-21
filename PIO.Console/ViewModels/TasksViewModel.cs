
using PIO.Bots.ClientLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.Modules;
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
		public override string Header => TranslationModule.Translate("Tasks");

		private int workerID;
		public TasksViewModel(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule, int WorkerID) : base(PIOClient, BotsClient,TranslationModule)
		{
			this.workerID = WorkerID;
		}

		protected override TaskViewModel OnCreateItem(PIO.Models.Task Model)
		{
			return new TaskViewModel(PIOClient, BotsClient,TranslationModule);
		}

		protected override async Task<IEnumerable<PIO.Models.Task>> OnLoadModelAsync()
		{
			return await PIOClient.GetTasksAsync(workerID);
		}
		

	}
}
