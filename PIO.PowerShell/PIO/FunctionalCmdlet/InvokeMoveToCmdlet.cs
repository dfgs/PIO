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
	[Cmdlet(VerbsLifecycle.Invoke, "MoveTo")]
	[OutputType(typeof(Task))]
	public class InvokeMoveToCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int WorkerID { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "ToPosition")]
		public int X { get; set; }
		[Parameter(Position = 2, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "ToPosition")]
		public int Y { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "ToFactoryID")]
		public int FactoryID { get; set; }





		protected override void ProcessRecord()
		{
			Task result;
			switch (this.ParameterSetName)
			{
				case "ToPosition":
					result = Try(() => client.MoveTo(WorkerID, X, Y));
					break;
				case "ToFactoryID":
					result = Try(() => client.MoveToFactory(WorkerID, FactoryID));
					break;
				default:
					throw new ArgumentException("Invalid parameter set.");
			}
			
			
			WriteObject(result);
		}

	

	}
}
