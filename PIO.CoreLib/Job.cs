using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Job:PIOData<JobID>, IJob
	{
		public required FactoryID FactoryID
		{
			get;
			set;
		}
		
		

		public Job()
		{
		}

		[SetsRequiredMembers]
		public Job(JobID ID,FactoryID FactoryID)
		{
			this.ID = ID;
			this.FactoryID = FactoryID;
		}
	}
}
