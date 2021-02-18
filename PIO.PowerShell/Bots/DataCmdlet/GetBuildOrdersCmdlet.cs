using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "BuildOrders")]
	[OutputType(typeof(BuildOrder[]))]
	public class GetBuildOrdersCmdlet : BotsCmdLet
	{

		

		protected override void ProcessRecord()
		{
			BuildOrder[] result;

			result = Try(() => client.GetBuildOrders());

			WriteObject(result);
		}

	

	}
}
