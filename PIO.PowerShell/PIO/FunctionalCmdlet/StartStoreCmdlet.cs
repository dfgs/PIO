﻿using PIO.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsLifecycle.Start, "Store")]
	[OutputType(typeof(Task))]
	public class StartStoreCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int WorkerID { get; set; }
	



		protected override void ProcessRecord()
		{
			Task result;

			result = Try(()=>client.Store(WorkerID));
			
			WriteObject(result);
		}

	

	}
}
