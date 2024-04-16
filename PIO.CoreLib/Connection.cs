using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Connection : PIOData<ConnectionID>, IConnection
	{
		public required ConnectorID SourceID
		{
			get;
			set;
		}

		public required ConnectorID DestinationID
		{
			get;
			set;
		}

		public Connection()
		{
		}

		[SetsRequiredMembers]
		public Connection(ConnectionID ID, ConnectorID SourceID, ConnectorID DestinationID)
		{
			this.ID= ID;
			this.SourceID = SourceID;
			this.DestinationID = DestinationID;
		}	

	}
}
