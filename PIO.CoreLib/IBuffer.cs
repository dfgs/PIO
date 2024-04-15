using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IBuffer:IETAProvider
	{
		float InRate
		{
			get;
		}
		float OutRate
		{
			get;
		}

		float InternalRate
		{
			get; 
		}
		float Capacity
		{
			get;
		}

		float Usage
		{
			get;
		}

		float SpaceLeft
		{
			get;
		}

		bool IsValid
		{
			get;
		}

		float GetCapacityAt(float Cycle);
		

	}
}
