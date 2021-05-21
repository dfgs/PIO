using PIO.Bots.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "HarvestOrder",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(HarvestOrder))]
	public class GetHarvestOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int HarvestOrderID { get; set; }




		protected override void ProcessRecord()
		{
			HarvestOrder result;

			result = Try(() => client.GetHarvestOrderAsync(HarvestOrderID).Result);

			WriteObject(result);
		}



	}
}
