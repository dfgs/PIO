using PIO.Bots.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "BuildOrder",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(BuildOrder))]
	public class GetBuildOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int BuildOrderID { get; set; }




		protected override void ProcessRecord()
		{
			BuildOrder result;

			result = Try(() => client.GetBuildOrderAsync(BuildOrderID).Result);

			WriteObject(result);
		}



	}
}
