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
	[Cmdlet(VerbsCommon.Get, "FarmType")]
	[OutputType(typeof(FarmType))]
	public class GetFarmTypeCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public FarmTypeIDs FarmTypeID { get; set; }


		

		protected override void ProcessRecord()
		{
			FarmType result;

			result = Try(() => client.GetFarmType(FarmTypeID));

			WriteObject(result);
		}

	

	}
}
