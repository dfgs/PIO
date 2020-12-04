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
	[Cmdlet(VerbsCommon.Get, "BuildingTypes")]
	[OutputType(typeof(BuildingType[]))]
	public class GetBuildingTypesCmdlet : PIOCmdLet
	{
			

		protected override void ProcessRecord()
		{
			BuildingType[] result;

			result = Try(() => client.GetBuildingTypes());

			WriteObject(result);
		}

	

	}
}
