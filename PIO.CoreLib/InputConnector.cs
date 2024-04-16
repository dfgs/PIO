﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class InputConnector : Connector, IInputConnector
	{

		public InputConnector() :base()
		{ 
		}

		[SetsRequiredMembers]
		public InputConnector(ConnectorID ID, FactoryID FactoryID, string ResourceType):base(ID,FactoryID,ResourceType)
		{

		}

	}
}
