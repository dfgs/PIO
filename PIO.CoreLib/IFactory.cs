using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IFactory:IPIOData<FactoryID>
	{
		string FactoryType
		{
			get;
		}

		IEnumerable<IInputConnector> Inputs
		{
			get;
		}
		IEnumerable<IOutputConnector> Outputs
		{
			get;
		}

		IEnumerable<IConnector> IOs
		{
			get;
		}


	}
}
