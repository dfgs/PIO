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
	public class FactoriesViewModel : RowsViewModel<FactoryViewModel>
	{
		public FactoriesViewModel(ILogger Logger, IPIOClient Client) : base(Logger,Client,()=>new FactoryViewModel(Logger,Client))
		{
		}

		
	}
}
