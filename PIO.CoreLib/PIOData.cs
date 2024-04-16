using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public abstract class PIOData<TId>:IPIOData<TId>
		where TId : struct
	{
		public required TId ID
		{
			get;
			set;
		}

		public PIOData()
		{
			
		}

	}
}
