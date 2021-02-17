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
	[Cmdlet(VerbsCommon.Get, "Materials")]
	[OutputType(typeof(Material[]))]
	public class GetMaterialsCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int MaterialSetID { get; set; }

		protected override void ProcessRecord()
		{
			Material[] result;

			result = Try(() => client.GetMaterials(MaterialSetID));

			WriteObject(result);
		}

	

	}
}
