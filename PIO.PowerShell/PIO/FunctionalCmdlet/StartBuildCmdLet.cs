using PIO.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsLifecycle.Start, "Build")]
	[OutputType(typeof(Task))]
	public class StartBuildCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int WorkerID { get; set; }
		
		
		
	


		protected override void ProcessRecord()
		{
			Task result;

			result = Try(()=>client.Build(WorkerID));
			
			WriteObject(result);
		}

	

	}
}
