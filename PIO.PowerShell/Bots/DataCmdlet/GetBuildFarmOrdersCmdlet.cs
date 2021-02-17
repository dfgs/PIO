using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "BuildFarmOrders")]
	[OutputType(typeof(BuildFarmOrder[]))]
	public class GetBuildFarmOrdersCmdlet : BotsCmdLet
	{

		

		protected override void ProcessRecord()
		{
			BuildFarmOrder[] result;

			result = Try(() => client.GetBuildFarmOrders());

			WriteObject(result);
		}

	

	}
}
