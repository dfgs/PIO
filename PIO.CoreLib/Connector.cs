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
	public class Connector:PIOData<ConnectorID>, IConnector
	{
		public required string ResourceType
		{
			get;
			set;
		}

		public IBuffer Buffer
		{
			get;
			set;
		}

		public Connector()
		{
			Buffer = new Buffer();
		}

		[SetsRequiredMembers]
		public Connector(ConnectorID ID,string ResourceType)
		{
			if (ResourceType == null) throw new PIOInvalidParameterException(nameof(ResourceType));
			this.ID = ID;
			this.ResourceType = ResourceType;
			Buffer = new Buffer();
		}
	}
}
