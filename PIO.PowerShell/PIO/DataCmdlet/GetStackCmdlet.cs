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
	[Cmdlet(VerbsCommon.Get, "Stack")]
	[OutputType(typeof(Stack))]
	public class GetStackCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int StackID { get; set; }


		

		protected override void ProcessRecord()
		{
			Stack result;

			result = Try(() => client.GetStack(StackID));

			WriteObject(result);
		}

	

	}
}
