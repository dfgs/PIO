using LogLib;
using NetORMLib;
using PIOClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib.ViewModels
{
	public abstract class RowViewModel : ViewModel<Row>
	{
		protected dynamic Model
		{
			get;
			private set;
		}

		public RowViewModel(ILogger Logger) : base( Logger)
		{
		}

		protected override sealed void OnSetModel(IPIOClient Client, Row Model)
		{
			this.Model = Model;
		}

	}
}
