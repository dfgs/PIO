using PIO.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "TaskType")]
	[OutputType(typeof(TaskType))]
	public class GetTaskTypeCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public TaskTypeIDs TaskTypeID { get; set; }


		

		protected override void ProcessRecord()
		{
			TaskType result;

			result = Try(() => client.GetTaskType(TaskTypeID));

			WriteObject(result);
		}

	

	}
}
