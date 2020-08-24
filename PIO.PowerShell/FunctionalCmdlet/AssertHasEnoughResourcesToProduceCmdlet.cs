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
	[Cmdlet(VerbsLifecycle.Assert, "HasEnoughResourcesToProduce")]
	[OutputType(typeof(bool))]
	public class AssertHasEnoughResourcesToProduceCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int FactoryID { get; set; }


		

		protected override void ProcessRecord()
		{
			bool result=false;
			try
			{
				result = client.HasEnoughResourcesToProduce(FactoryID);
			}
			catch(FaultException<PIOFault> ex)
			{
				
				ThrowTerminatingError(new ErrorRecord(
					 ex,ex.Detail.Message,ErrorCategory.InvalidOperation,null));
			}
			WriteObject(result);
		}

	

	}
}
