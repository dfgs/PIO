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
	[Cmdlet(VerbsCommon.Get, "ResourceTypes")]
	[OutputType(typeof(ResourceType[]))]
	public class GetResourceTypesCmdlet : PIOCmdLet
	{
			

		protected override void ProcessRecord()
		{
			ResourceType[] result;

			result = Try(() => client.GetResourceTypes());

			WriteObject(result);
		}

	

	}
}
