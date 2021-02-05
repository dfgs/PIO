using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "BuildFactoryOrders")]
	[OutputType(typeof(BuildFactoryOrder[]))]
	public class GetBuildFactoryOrdersCmdlet : BotsCmdLet
	{

		

		protected override void ProcessRecord()
		{
			BuildFactoryOrder[] result;

			result = Try(() => client.GetBuildFactoryOrders());

			WriteObject(result);
		}

	

	}
}
