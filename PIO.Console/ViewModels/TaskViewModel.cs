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
	public class TaskViewModel : PIOViewModel<PIO.Models.Task>
	{
		

		public string Name
		{
			get { return PhrasesViewModel.GetString(Model.TaskTypeID.ToString()); }
		}

		public TaskViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient,PhrasesViewModel PhrasesViewModel) : base(PIOClient, BotsClient, PhrasesViewModel)
		{
			
		}

		

		protected override async Task<PIO.Models.Task> OnLoadModelAsync()
		{
			return await PIOClient.GetTaskAsync(Model.TaskID);
		}

	}
}
