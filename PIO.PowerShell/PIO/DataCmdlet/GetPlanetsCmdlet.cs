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
	[Cmdlet(VerbsCommon.Get, "Planets")]
	[OutputType(typeof(Planet[]))]
	public class GetPlanetsCmdlet : PIOCmdLet
	{
			

		protected override void ProcessRecord()
		{
			Planet[] result;

			result = Try(() => client.GetPlanets());

			WriteObject(result);
		}

	

	}
}
