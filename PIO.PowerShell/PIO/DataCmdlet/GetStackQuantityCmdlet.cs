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
	[Cmdlet(VerbsCommon.Get, "StackQuantity")]
	[OutputType(typeof(int))]
	public class GetStackQuantityCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int BuildingID { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public ResourceTypeIDs ResourceTypeID { get; set; }

		protected override void ProcessRecord()
		{
			int result;

			result = Try(() => client.GetStackQuantity(BuildingID, ResourceTypeID));

			WriteObject(result);
		}

	

	}
}
