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
	[Cmdlet(VerbsCommon.Get, "Product")]
	[OutputType(typeof(Product))]
	public class GetProductCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int ProductID { get; set; }


		

		protected override void ProcessRecord()
		{
			Product result;

			result = client.GetProduct(ProductID);

			WriteObject(result);
		}

	

	}
}
