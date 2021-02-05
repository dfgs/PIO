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
	[Cmdlet(VerbsCommon.Get, "MissingResourcesToBuild")]
	[OutputType(typeof(ResourceTypeIDs[]))]
	public class GetMissingResourcesToBuildCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int FactoryID { get; set; }


		

		protected override void ProcessRecord()
		{
			ResourceTypeIDs[] result ;

			result = Try(()=>client.GetMissingResourcesToBuild(FactoryID));
			
			WriteObject(result);
		}

	

	}
}
