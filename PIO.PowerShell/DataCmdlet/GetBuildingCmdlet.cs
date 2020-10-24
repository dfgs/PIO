using PIO.Models;
using PIO.PowerShell.PIOWebServiceReference;
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
	[Cmdlet(VerbsCommon.Get, "Building")]
	[OutputType(typeof(Building))]
	public class GetBuildingCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int BuildingID { get; set; }


		

		protected override void ProcessRecord()
		{
			Building result;

			result = Try(() => client.GetBuilding(BuildingID));

			WriteObject(result);
		}

	

	}
}
