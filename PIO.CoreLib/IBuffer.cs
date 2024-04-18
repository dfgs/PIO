using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IBuffer:IETAProvider,IPIOData<BufferID>
	{
		ConnectorID ConnectorID
		{
			get; 
		}

		float InRate
		{
			get;
			set;
		}
		float OutRate
		{
			get;
			set;
		}

		float InternalRate
		{
			get; 
		}
		float Capacity
		{
			get;
		}

		float InitialUsage
		{
			get;
		}

		float InitialSpaceLeft
		{
			get;
		}

		bool IsValid
		{
			get;
		}

		float GetUsageAt(float Cycle);
		
		void Update(float Cycle);
	}
}
