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
	[Cmdlet(VerbsCommon.Get, "Phrases")]
	[OutputType(typeof(Phrase[]))]
	public class GetPhrasesCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public string CountryCode { get; set; }


		protected override void ProcessRecord()
		{
			Phrase[] result;

			result = Try(() => client.GetPhrases(CountryCode));

			WriteObject(result);
		}

	

	}
}
