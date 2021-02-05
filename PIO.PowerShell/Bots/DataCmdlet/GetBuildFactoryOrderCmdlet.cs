using PIO.Bots.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "BuildFactoryOrder",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(BuildFactoryOrder))]
	public class GetBuildFactoryOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int BuildFactoryOrderID { get; set; }




		protected override void ProcessRecord()
		{
			BuildFactoryOrder result;

			result = Try(() => client.GetBuildFactoryOrder(BuildFactoryOrderID));

			WriteObject(result);
		}



	}
}
