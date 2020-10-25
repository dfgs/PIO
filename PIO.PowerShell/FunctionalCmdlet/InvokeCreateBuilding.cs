using PIO.Models;
using PIO.PowerShell.PIOWebServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsLifecycle.Invoke, "CreateBuilding")]
	[OutputType(typeof(Task))]
	public class InvokeCreateBuildingCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int WorkerID { get; set; }
		
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }
		
		[Parameter(Position = 2, ValueFromPipeline = true, Mandatory = true)]
		public FactoryTypeIDs FactoryTypeID { get; set; }
		
	


		protected override void ProcessRecord()
		{
			Task result;

			result = Try(()=>client.CreateBuilding(WorkerID,PlanetID,FactoryTypeID));
			
			WriteObject(result);
		}

	

	}
}
