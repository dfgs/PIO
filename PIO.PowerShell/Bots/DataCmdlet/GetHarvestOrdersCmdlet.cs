using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "HarvestOrders")]
	[OutputType(typeof(HarvestOrder[]))]
	public class GetHarvestOrdersCmdlet : BotsCmdLet
	{

		

		protected override void ProcessRecord()
		{
			HarvestOrder[] result;

			result = Try(() => client.GetHarvestOrders());

			WriteObject(result);
		}

	

	}
}
