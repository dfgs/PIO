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
	public class StackViewModel : RowViewModel<StackRow>
	{
		public int StackID
		{
			get { return Model.StackID; }
		}
		public int ResourceID
		{
			get { return Model.ResourceID; }
		}
		public int Quantity
		{
			get { return Model.Quantity; }
		}
		

		public StackViewModel(ILogger Logger, IPIOClient Client) : base(Logger,Client)
		{
		}

		


	}
}
