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
	[Cmdlet(VerbsCommon.Get, "Material")]
	[OutputType(typeof(Material))]
	public class GetMaterialCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int MaterialID { get; set; }


		

		protected override void ProcessRecord()
		{
			Material result;

			result = Try(() => client.GetMaterial(MaterialID));

			WriteObject(result);
		}

	

	}
}
