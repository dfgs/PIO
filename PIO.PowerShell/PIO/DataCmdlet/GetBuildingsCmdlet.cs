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
	[Cmdlet(VerbsCommon.Get, "Buildings")]
	[OutputType(typeof(Factory[]))]
	public class GetBuildingsCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }

		protected override void ProcessRecord()
		{
			Building[] result;

			result = Try(() => client.GetBuildings(PlanetID));

			WriteObject(result);
		}

	

	}
}
