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
	[Cmdlet(VerbsCommon.Get, "Building",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(Building))]
	public class GetBuildingCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true,ParameterSetName ="FromID")]
		public int BuildingID { get; set; }


		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int X { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int Y { get; set; }



		protected override void ProcessRecord()
		{
			Building result;
			
			switch (this.ParameterSetName)
			{
				case "FromID":
					result = Try(() => client.GetBuilding(BuildingID));
					break;
				case "FromPosition":
					result = Try(() => client.GetBuildingAtPos(X,Y));
					break;
				default:
					throw new ArgumentException("Invalid parameter set.");
			}

			

			WriteObject(result);
		}

	

	}
}
