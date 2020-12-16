using PIO.ClientLib.PIOServiceReference;
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
	public abstract class BaseCmdLet<ClientT>:PSCmdlet
	{
		[Parameter(ValueFromPipeline = false, Mandatory = false)]
		public string Server { get; set; } = "127.0.0.1";

		protected ClientT client;

		protected T Try<T>(Func<T> Func)
		{
			try
			{
				return Func();
			}
			catch(FaultException ex)
			{
				ErrorRecord record = new ErrorRecord(ex, ex.Code.Name, ErrorCategory.NotSpecified, null);
				ThrowTerminatingError(record);
			}
			catch (Exception ex)
			{
				ErrorRecord record = new ErrorRecord(ex, "Undefined", ErrorCategory.NotSpecified, null);
				ThrowTerminatingError(record);
			}
			return default(T);
		}

		protected abstract ClientT OnCreateClient();
		protected abstract void OnCloseClient();

		protected override void BeginProcessing()
		{
			base.BeginProcessing();

			client = OnCreateClient();
		}

		protected override void EndProcessing()
		{
			// finalizing
			base.EndProcessing();
			OnCloseClient();
		}
		protected override void StopProcessing()
		{
			// abnormal
			base.StopProcessing();
			OnCloseClient();
		}


	}
}
