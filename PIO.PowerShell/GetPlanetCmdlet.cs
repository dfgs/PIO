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
	[Cmdlet(VerbsCommon.Get, "Planet")]
	[OutputType(typeof(Planet))]
	public class GetPlanetCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int PlanetID { get; set; }


		

		protected override void ProcessRecord()
		{
			Planet planet;

			planet = client.GetPlanet(PlanetID);

			WriteObject(planet);
		}

	

	}
}
