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
		public required IOutputConnector Source
		{
			get;
			set;
		}

		public required IInputConnector Destination
		{
			get;
			set;
		}

		public Connection()
		{
		}

		[SetsRequiredMembers]
		public Connection(ConnectionID ID, IOutputConnector Source, IInputConnector Destination)
		{
			if (Source == null) throw new PIOInvalidParameterException(nameof(Source));
			if (Destination == null) throw new PIOInvalidParameterException(nameof(Destination));
			this.ID= ID;
			this.Source = Source;
			this.Destination = Destination;
		}	

	}
}
