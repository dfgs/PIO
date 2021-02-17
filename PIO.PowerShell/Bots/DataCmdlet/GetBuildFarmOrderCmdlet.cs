using PIO.Bots.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "BuildFarmOrder",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(BuildFarmOrder))]
	public class GetBuildFarmOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int BuildFarmOrderID { get; set; }




		protected override void ProcessRecord()
		{
			BuildFarmOrder result;

			result = Try(() => client.GetBuildFarmOrder(BuildFarmOrderID));

			WriteObject(result);
		}



	}
}
