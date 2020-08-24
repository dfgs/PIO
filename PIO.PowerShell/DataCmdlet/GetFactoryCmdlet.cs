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
	[Cmdlet(VerbsCommon.Get, "Factory")]
	[OutputType(typeof(Factory))]
	public class GetFactoryCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int FactoryID { get; set; }


		

		protected override void ProcessRecord()
		{
			Factory result;

			result = client.GetFactory(FactoryID);

			WriteObject(result);
		}

	

	}
}
