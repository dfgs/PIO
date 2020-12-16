using PIO.Bots.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "ProduceOrder",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(ProduceOrder))]
	public class GetProduceOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int ProduceOrderID { get; set; }




		protected override void ProcessRecord()
		{
			ProduceOrder result;

			result = Try(() => client.GetProduceOrder(ProduceOrderID));

			WriteObject(result);
		}



	}
}
