using LogLib;
using NetORMLib;
using PIOClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib
{
	public abstract class RowViewModel : ViewModel<Row>
	{
		public RowViewModel(ILogger Logger, IPIOClient Client) : base( Logger, Client)
		{
		}
	}
}
