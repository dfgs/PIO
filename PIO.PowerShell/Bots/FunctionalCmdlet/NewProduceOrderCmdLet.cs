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
	[Cmdlet(VerbsCommon.New, "ProduceOrder")]
	[OutputType(typeof(ProduceOrder))]
	public class NewProduceOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }


		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public int FactoryID { get; set; }





		protected override void ProcessRecord()
		{
			ProduceOrder result;

			result = Try(()=>client.CreateProduceOrderAsync(PlanetID,FactoryID).Result);
			
			WriteObject(result);
		}

	

	}
}
