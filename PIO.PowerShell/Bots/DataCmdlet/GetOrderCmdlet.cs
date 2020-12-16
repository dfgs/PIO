using PIO.Bots.Models;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "Order",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(Order))]
	public class GetOrderCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int OrderID { get; set; }




		protected override void ProcessRecord()
		{
			Order result;

			result = Try(() => client.GetOrder(OrderID));

			WriteObject(result);
		}



	}
}
