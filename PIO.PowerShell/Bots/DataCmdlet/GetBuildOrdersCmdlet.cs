using PIO.Bots.Models;
using PIO.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "BuildOrders")]
	[OutputType(typeof(BuildOrder[]))]
	public class GetBuildOrdersCmdlet : BotsCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }

		protected override void ProcessRecord()
		{
			BuildOrder[] result;

			result = Try(() => client.GetBuildOrdersAsync(PlanetID).Result);

			WriteObject(result);
		}

	

	}
}
