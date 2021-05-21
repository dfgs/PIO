using PIO.Bots.Models;
using System;
using System.Management.Automation;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "Bot",DefaultParameterSetName ="FromID")]
	[OutputType(typeof(Bot))]
	public class GetBotCmdlet : BotsCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromID")]
		public int BotID { get; set; }
		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromWorkerID")]
		public int WorkerID { get; set; }




		protected override void ProcessRecord()
		{
			Bot result;

			switch (this.ParameterSetName)
			{
				case "FromID":
					result = Try(() => client.GetBotAsync(BotID).Result);
					break;
				case "FromWorkerID":
					result = Try(() => client.GetBotForWorkerAsync(WorkerID).Result);
					break;
				default:
					throw new ArgumentException("Invalid parameter set.");
			}


			WriteObject(result);
		}



	}
}
