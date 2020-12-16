using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "Orders")]
	[OutputType(typeof(Order[]))]
	public class GetOrdersCmdlet : BotsCmdLet
	{

		

		protected override void ProcessRecord()
		{
			Order[] result;

			result = Try(() => client.GetOrders());

			WriteObject(result);
		}

	

	}
}
