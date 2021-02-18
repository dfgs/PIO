using PIO.Bots.Models;
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
	[Cmdlet(VerbsCommon.New, "BuildOrder")]
	[OutputType(typeof(BuildOrder))]
	public class NewBuildOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }


		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public BuildingTypeIDs BuildingTypeID { get; set; }

		[Parameter(Position = 2, ValueFromPipeline = true, Mandatory = true)]
		public int X { get; set; }
		[Parameter(Position = 3, ValueFromPipeline = true, Mandatory = true)]
		public int Y { get; set; }





		protected override void ProcessRecord()
		{
			BuildOrder result;

			result = Try(()=>client.CreateBuildOrder(PlanetID, BuildingTypeID, X,Y));
			
			WriteObject(result);
		}

	

	}
}
