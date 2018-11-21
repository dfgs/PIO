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

		public FactoryViewModel(ILogger Logger) : base(Logger)
		{
			Stacks = new StacksViewModel(Logger);
		}
		protected override void OnLoaded(IPIOClient Client)
		{
			Stacks.Load(Client, Client.GetStacks(this.FactoryID));
		}




	}
}
