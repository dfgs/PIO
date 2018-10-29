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

		public FactoryViewModel(ILogger Logger, IPIOClient Client) : base(Logger, Client)
		{
		}

		


	}
}
