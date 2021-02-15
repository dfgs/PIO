using PIO.Models;

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
	[Cmdlet(VerbsCommon.Get, "Farm", DefaultParameterSetName = "FromID")]
	[OutputType(typeof(Farm))]
	public class GetFarmCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true, ParameterSetName = "FromID")]
		public int FarmID { get; set; }


		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int PlanetID { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int X { get; set; }
		[Parameter(Position = 2, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int Y { get; set; }


		protected override void ProcessRecord()
		{
			Farm result;

			switch (this.ParameterSetName)
			{
				case "FromID":
					result=Try(() => client.GetFarm(FarmID));
					break;
				case "FromPosition":
					result=Try(() => client.GetFarmAtPos(PlanetID,X,Y));
					break;
				default:
					throw new ArgumentException("Invalid parameter set.");
			}

			WriteObject(result);
		}

	

	}
}
