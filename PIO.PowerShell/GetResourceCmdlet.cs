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
	[Cmdlet(VerbsCommon.Get, "Resource")]
	[OutputType(typeof(Resource))]
	public class GetResourceCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int ResourceID { get; set; }


		

		protected override void ProcessRecord()
		{
			Resource result;

			result = client.GetResource(ResourceID);

			WriteObject(result);
		}

	

	}
}
