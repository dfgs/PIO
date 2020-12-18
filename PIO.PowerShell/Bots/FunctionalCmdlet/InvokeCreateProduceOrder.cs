using PIO.Bots.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsLifecycle.Invoke, "CreateProduceOrder")]
	[OutputType(typeof(ProduceOrder))]
	public class InvokeCreateProduceOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int FactoryID { get; set; }
		
		
		
	


		protected override void ProcessRecord()
		{
			ProduceOrder result;

			result = Try(()=>client.CreateProduceOrder(FactoryID));
			
			WriteObject(result);
		}

	

	}
}
