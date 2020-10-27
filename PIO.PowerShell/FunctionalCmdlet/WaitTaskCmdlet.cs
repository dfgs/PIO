using PIO.Models;
using PIO.PowerShell.PIOWebServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsLifecycle.Wait, "Task")]
	[OutputType(typeof(Task))]
	public class WaitTaskCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public Task Task { get; set; }
	




		protected override void ProcessRecord()
		{
			if (Task!=null) Thread.Sleep((int)(Task.ETA - DateTime.Now).TotalMilliseconds+1000);
			
			WriteObject(Task);
		}

	

	}
}
