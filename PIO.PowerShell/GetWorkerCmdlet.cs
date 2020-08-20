using PIO.Models;
using PIO.PowerShell.PIOWebServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "Worker")]
	[OutputType(typeof(Worker))]
	public class GetWorkerCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int WorkerID { get; set; }


		

		protected override void ProcessRecord()
		{
			Worker result;

			result = client.GetWorker(WorkerID);

			WriteObject(result);
		}

	

	}
}
