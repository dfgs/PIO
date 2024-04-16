﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IConnector : IPIOData<ConnectorID>
	{
		string ResourceType
		{
			get;
		}

		IBuffer Buffer
		{
			get;
		}

	}
}
