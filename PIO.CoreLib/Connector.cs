using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Connector:IConnector
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
		public Connector(string ResourceType)
		{
			this.ResourceType = ResourceType;
			Buffer = new Buffer();
		}
	}
}
