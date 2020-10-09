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
	[Cmdlet(VerbsCommon.Get, "Products")]
	[OutputType(typeof(Product[]))]
	public class GetProductsCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public FactoryTypeIDs FactoryTypeID { get; set; }

		protected override void ProcessRecord()
		{
			Product[] result;

			result = Try(() => client.GetProducts(FactoryTypeID));

			WriteObject(result);
		}

	

	}
}
