using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "Bots")]
	[OutputType(typeof(Bot[]))]
	public class GetBotsCmdlet : BotsCmdLet
	{

		

		protected override void ProcessRecord()
		{
			Bot[] result;

			result = Try(() => client.GetBotsAsync().Result);

			WriteObject(result);
		}

	

	}
}
