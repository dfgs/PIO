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
	[Cmdlet(VerbsCommon.Get, "ResourceType")]
	[OutputType(typeof(ResourceType))]
	public class GetResourceTypeCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public ResourceTypeIDs ResourceTypeID { get; set; }


		

		protected override void ProcessRecord()
		{
			ResourceType result;

			result = Try(() => client.GetResourceType(ResourceTypeID));

			WriteObject(result);
		}

	

	}
}
