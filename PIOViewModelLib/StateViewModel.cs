using LogLib;
using NetORMLib;
using PIOClientLib;
using PIOViewModelLib;
using PIOViewModelLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOViewModelLib
{
	public class StateViewModel : RowViewModel
	{
		
		public int StateID
		{
			get { return Model.StateID; }
		}
		public string Name
		{
			get { return Model.Name; }
		}

		public StateViewModel(ILogger Logger, IPIOClient Client) : base(Logger,Client)
		{
		}

		


	}
}
