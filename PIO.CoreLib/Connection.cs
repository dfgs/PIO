using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Connection : IConnection
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
		public Connection(IOutputConnector Source, IInputConnector Destination)
		{
			this.Source = Source;
			this.Destination = Destination;
		}	

	}
}
