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
	[Cmdlet(VerbsCommon.Get, "Stacks")]
	[OutputType(typeof(Stack[]))]
	public class GetStacksCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int BuildingID { get; set; }

		protected override void ProcessRecord()
		{
			Stack[] result;

			result = Try(() => client.GetStacks(BuildingID));

			WriteObject(result);
		}

	

	}
}
