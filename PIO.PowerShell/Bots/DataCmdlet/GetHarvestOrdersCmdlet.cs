using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "HarvestOrders")]
	[OutputType(typeof(HarvestOrder[]))]
	public class GetHarvestOrdersCmdlet : BotsCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }

		protected override void ProcessRecord()
		{
			HarvestOrder[] result;

			result = Try(() => client.GetHarvestOrders(PlanetID));

			WriteObject(result);
		}

	

	}
}
