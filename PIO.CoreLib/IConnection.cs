﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IConnection : IPIOData<ConnectionID>
	{
		IOutputConnector Source
		{
			get;
		}

		IInputConnector Destination
		{
			get;
		}

		

	}
}
