using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "ProduceOrders")]
	[OutputType(typeof(ProduceOrder[]))]
	public class GetProduceOrdersCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }


		protected override void ProcessRecord()
		{
			ProduceOrder[] result;

			result = Try(() => client.GetProduceOrders(PlanetID));

			WriteObject(result);
		}

	

	}
}
