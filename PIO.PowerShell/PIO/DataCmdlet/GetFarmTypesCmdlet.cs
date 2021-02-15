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
	[Cmdlet(VerbsCommon.Get, "FarmTypes")]
	[OutputType(typeof(FarmType[]))]
	public class GetFarmTypesCmdlet : PIOCmdLet
	{
			

		protected override void ProcessRecord()
		{
			FarmType[] result;

			result = Try(() => client.GetFarmTypes());

			WriteObject(result);
		}

	

	}
}
