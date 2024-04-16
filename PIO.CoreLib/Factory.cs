using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Factory:PIOData<FactoryID>, IFactory
	{
		
		public required string FactoryType
		{
			get;
			set;
		}

		IEnumerable<IInputConnector> IFactory.Inputs => Inputs;
		public List<IInputConnector> Inputs
		{
			get;
			set;
		}

		IEnumerable<IOutputConnector> IFactory.Outputs => Outputs;
		public List<IOutputConnector> Outputs
		{
			get;
			set;
		}

		public IEnumerable<IConnector> IOs
		{
			get
			{
				foreach (IConnector connector in Inputs) yield return connector;
				foreach (IConnector connector in Outputs) yield return connector;
			}
		}

		public Factory() 
		{
			Inputs = new List<IInputConnector>();
			Outputs = new List<IOutputConnector>();
		}
		[SetsRequiredMembers]
		public Factory(FactoryID ID, string FactoryType)
		{
			if (FactoryType == null) throw new PIOInvalidParameterException(nameof(FactoryType));
			this.ID= ID;
			this.FactoryType = FactoryType;
			Inputs = new List<IInputConnector>();
			Outputs = new List<IOutputConnector>();
		}


	}
}
