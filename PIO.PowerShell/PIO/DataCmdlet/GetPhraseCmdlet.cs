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
	[Cmdlet(VerbsCommon.Get, "Phrase")]
	[OutputType(typeof(Phrase))]
	public class GetPhraseCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public string Key { get; set; }

		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public string CountryCode { get; set; }




		protected override void ProcessRecord()
		{
			Phrase result;

			result = Try(() => client.GetPhrase(Key,CountryCode));

			WriteObject(result);
		}

	

	}
}
