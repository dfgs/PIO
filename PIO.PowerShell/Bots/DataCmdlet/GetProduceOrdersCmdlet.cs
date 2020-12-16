using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "ProduceOrders")]
	[OutputType(typeof(ProduceOrder[]))]
	public class GetProduceOrdersCmdlet : BotsCmdLet
	{

		

		protected override void ProcessRecord()
		{
			ProduceOrder[] result;

			result = Try(() => client.GetProduceOrders());

			WriteObject(result);
		}

	

	}
}
