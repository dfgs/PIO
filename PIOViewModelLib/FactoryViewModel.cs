using LogLib;
using NetORMLib;
using PIOClientLib;
using PIOViewModelLib;
using PIOViewModelLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib
{
	public class FactoryViewModel : RowViewModel
	{
		public int FactoryID
		{
			get { return Model.FactoryID; }
		}
		public string Name
		{
			get { return Model.Name; }
		}
		public StacksViewModel Stacks
		{
			get;
			private set;
		}
		public TaskViewModel Task
		{
			get;
			private set;
		}

		public IEnumerable<object> ItemsSource
		{
			get
			{
				foreach (StackViewModel item in Stacks) yield return item;
				yield return Task;
			}
		}
		public FactoryViewModel(ILogger Logger, IPIOClient Client) : base(Logger,Client)
		{
			Stacks = new StacksViewModel(Logger,Client);
			Task = new TaskViewModel(Logger, Client);
		}

		protected override void OnLoaded()
		{
			Stacks.Load(()=>Client.GetStacks(this.FactoryID));
			Task.Load(() => Client.GetTask(this.FactoryID));
		}




	}
}
