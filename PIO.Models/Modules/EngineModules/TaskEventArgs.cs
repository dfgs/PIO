using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public class TaskEventArgs:EventArgs
	{
		public Task Task
		{
			get;
			private set;
		}

		public TaskEventArgs(Task Task)
		{
			this.Task = Task;
		}
	}

	public delegate void TaskEventHandler(object sender, TaskEventArgs e);
}
