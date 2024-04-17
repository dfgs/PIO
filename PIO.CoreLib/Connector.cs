using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public abstract class Connector:PIOData<ConnectorID>, IConnector
	{
		public required FactoryID FactoryID
		{
			get;
			set;
		}
		public required string ResourceType
		{
			get;
			set;
		}
		

		

		public Connector()
		{
		}

		[SetsRequiredMembers]
		public Connector(ConnectorID ID,FactoryID FactoryID, string ResourceType)
		{
			if (ResourceType == null) throw new PIOInvalidParameterException(nameof(ResourceType));

			this.ID = ID;
			this.FactoryID = FactoryID;
			this.ResourceType = ResourceType;
		}
	}
}
