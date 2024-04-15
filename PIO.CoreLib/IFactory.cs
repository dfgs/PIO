﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IFactory
	{
		string FactoryType
		{
			get;
		}

		IEnumerable<IConnector> Inputs
		{
			get;
		}
		IEnumerable<IConnector> Outputs
		{
			get;
		}

		IEnumerable<IConnector> IOs
		{
			get;
		}


	}
}
