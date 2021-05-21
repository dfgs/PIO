
using PIO.Bots.ClientLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.Console.Modules;
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
	public class TaskViewModel : PIOViewModel<PIO.Models.Task>
	{
		public override string Header => TranslationModule.Translate("Task");


		public string Name
		{
			get { return TranslationModule.Translate(Model.TaskTypeID.ToString()); }
		}

		public TaskViewModel(PIOServiceClient PIOClient, BotsRESTClient BotsClient, ITranslationModule TranslationModule) : base(PIOClient, BotsClient,TranslationModule)
		{
			
		}

		

		protected override async Task<PIO.Models.Task> OnLoadModelAsync()
		{
			return await PIOClient.GetTaskAsync(Model.TaskID);
		}

	}
}
