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
	public class StacksViewModel : RowsViewModel<StackViewModel>
	{
		public StacksViewModel(ILogger Logger, IPIOClient Client) : base(Logger,Client, ()=>new StackViewModel(Logger,Client))
		{
		}
		
	}
}
