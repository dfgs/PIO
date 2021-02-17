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
	[Cmdlet(VerbsCommon.New, "Farm")]
	[OutputType(typeof(Task))]
	public class NewFarmCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int WorkerID { get; set; }
		
		
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public FarmTypeIDs FarmTypeID { get; set; }
		
	


		protected override void ProcessRecord()
		{
			Task result;

			result = Try(()=>client.CreateFarm(WorkerID,FarmTypeID));
			
			WriteObject(result);
		}

	

	}
}
