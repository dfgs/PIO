using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Factory:IFactory
	{
		public required string FactoryType
		{
			get;
			set;
		}

		IEnumerable<IConnector> IFactory.Inputs => Inputs;
		public List<IConnector> Inputs
		{
			get;
			set;
		}

		IEnumerable<IConnector> IFactory.Outputs => Outputs;
		public List<IConnector> Outputs
		{
			get;
			set;
		}

		public IEnumerable<IConnector> IOs => Inputs.Concat(Outputs);

		public Factory() 
		{
			Inputs = new List<IConnector>();
			Outputs = new List<IConnector>();
		}
		[SetsRequiredMembers]
		public Factory(string FactoryType)
		{
			this.FactoryType = FactoryType;
			Inputs = new List<IConnector>();
			Outputs = new List<IConnector>();
		}


	}
}
