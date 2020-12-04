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
	[Cmdlet(VerbsCommon.Get, "TaskTypes")]
	[OutputType(typeof(TaskType[]))]
	public class GetTaskTypesCmdlet : PIOCmdLet
	{
			

		protected override void ProcessRecord()
		{
			TaskType[] result;

			result = Try(() => client.GetTaskTypes());

			WriteObject(result);
		}

	

	}
}
