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
	public class Worker:PIOData<WorkerID>, IWorker
	{
		
	

		public Worker() 
		{
		}
		[SetsRequiredMembers]
		public Worker(WorkerID ID)
		{
			this.ID = ID;
		}



	}
}
