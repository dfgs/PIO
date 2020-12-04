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
	[Cmdlet(VerbsCommon.Get, "FactoryTypes")]
	[OutputType(typeof(FactoryType[]))]
	public class GetFactoryTypesCmdlet : PIOCmdLet
	{
			

		protected override void ProcessRecord()
		{
			FactoryType[] result;

			result = Try(() => client.GetFactoryTypes());

			WriteObject(result);
		}

	

	}
}
