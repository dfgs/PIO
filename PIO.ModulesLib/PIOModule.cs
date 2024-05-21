using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public abstract class PIOModule:Module,IPIOModule
	{
		protected IDataSource DataSource
		{
			get;
			private set;
		}

		public PIOModule(ILogger Logger,IDataSource DataSource):base(Logger)
		{
			if (DataSource==null) throw new PIOInvalidParameterException(nameof(DataSource));
			this.DataSource = DataSource;
		}

	}
}
