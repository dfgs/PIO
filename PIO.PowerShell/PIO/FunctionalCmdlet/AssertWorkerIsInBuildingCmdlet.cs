﻿using PIO.Models;

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
	[Cmdlet(VerbsLifecycle.Assert, "WorkerIsInBuilding")]
	[OutputType(typeof(bool))]
	public class AssertWorkerIsInBuildingCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int WorkerID { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public int BuildingID { get; set; }




		protected override void ProcessRecord()
		{
			bool result=false;

			result = Try(()=>client.WorkerIsInBuilding(WorkerID,BuildingID));
			
			WriteObject(result);
		}

	

	}
}
