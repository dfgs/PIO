using LogLib;
using NetORMLib;
using PIOClientLib;
using PIOServerLib.Rows;
using PIOViewModelLib;
using PIOViewModelLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib
{
	public class TaskViewModel : RowViewModel<TaskRow>
	{
		public int TaskID
		{
			get { return Model.TaskID; }
		}
		public string Name
		{
			get { return Model.Name; }
		}
		
		

		public TaskViewModel(ILogger Logger, IPIOClient Client) : base(Logger,Client)
		{
		}

		


	}
}
