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
	[Cmdlet(VerbsCommon.Get, "Task")]
	[OutputType(typeof(Task))]
	public class GetTaskCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int TaskID { get; set; }


		

		protected override void ProcessRecord()
		{
			Task result;

			result = Try(() => client.GetTask(TaskID));

			WriteObject(result);
		}

	

	}
}
