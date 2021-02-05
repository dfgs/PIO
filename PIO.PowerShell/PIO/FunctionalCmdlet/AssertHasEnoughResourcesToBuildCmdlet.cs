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
	[Cmdlet(VerbsLifecycle.Assert, "HasEnoughResourcesToBuild")]
	[OutputType(typeof(bool))]
	public class AssertHasEnoughResourcesToBuildCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int FactoryID { get; set; }


		

		protected override void ProcessRecord()
		{
			bool result=false;

			result = Try(()=>client.HasEnoughResourcesToBuild(FactoryID));
			
			WriteObject(result);
		}

	

	}
}
