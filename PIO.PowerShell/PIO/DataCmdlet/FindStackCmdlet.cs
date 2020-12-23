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
	[Cmdlet(VerbsCommon.Find, "Stack")]
	[OutputType(typeof(Stack))]
	public class FindStackCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }

		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public ResourceTypeIDs ResourceTypeID { get; set; }




		protected override void ProcessRecord()
		{
			Stack result;

			result = Try(() => client.FindStack(PlanetID, ResourceTypeID));

			WriteObject(result);
		}

	

	}
}
