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
	public class SubTask:PIOData<SubTaskID>, ISubTask
	{
		public required JobID JobID
		{
			get;
			set;
		}
		
		

		public SubTask()
		{
		}

		[SetsRequiredMembers]
		public SubTask(SubTaskID ID,JobID JobID)
		{
			this.ID = ID;
			this.JobID = JobID;
		}
	}
}
