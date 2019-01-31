using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Executors
{
	public interface IExecutor
	{
		int TaskID
		{
			get;
		}

		int Execute(int FactoryID);
		
	}
}
