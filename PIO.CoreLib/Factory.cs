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


		public Factory() 
		{
		}

		[SetsRequiredMembers]
		public Factory(FactoryID ID, string FactoryType)
		{
			if (FactoryType == null) throw new PIOInvalidParameterException(nameof(FactoryType));
			this.ID= ID;
			this.FactoryType = FactoryType;
		}


	}
}
