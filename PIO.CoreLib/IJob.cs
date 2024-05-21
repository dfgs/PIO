using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IJob : IPIOData<JobID>
	{
		FactoryID FactoryID
		{
			get;
		}
		
	
	}
}
