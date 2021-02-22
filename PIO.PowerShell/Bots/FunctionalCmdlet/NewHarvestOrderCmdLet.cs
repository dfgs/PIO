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
	[Cmdlet(VerbsCommon.New, "HarvestOrder")]
	[OutputType(typeof(HarvestOrder))]
	public class NewHarvestOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }


		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public int FactoryID { get; set; }





		protected override void ProcessRecord()
		{
			HarvestOrder result;

			result = Try(()=>client.CreateHarvestOrder(PlanetID,FactoryID));
			
			WriteObject(result);
		}

	

	}
}
