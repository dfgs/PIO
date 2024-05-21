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
		
		public required FactoryTypeID FactoryTypeID
		{
			get;
			set;
		}

		public float Efficiency
		{
			get;
			set;
		}

		public Factory() 
		{
		}

		[SetsRequiredMembers]
		public Factory(FactoryID ID, FactoryTypeID FactoryTypeID)
		{
			this.ID= ID;
			this.FactoryTypeID = FactoryTypeID;
		}


	}
}
