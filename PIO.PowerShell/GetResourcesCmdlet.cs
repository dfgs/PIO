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
	[Cmdlet(VerbsCommon.Get, "Resources")]
	[OutputType(typeof(Resource[]))]
	public class GetResourcesCmdlet : PIOCmdLet
	{
			

		protected override void ProcessRecord()
		{
			Resource[] result;

			result = client.GetResources();

			WriteObject(result);
		}

	

	}
}
