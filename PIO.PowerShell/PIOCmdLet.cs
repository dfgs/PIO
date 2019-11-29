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
	public class PIOCmdLet:Cmdlet
	{
		[Parameter(ValueFromPipeline = false, Mandatory = false)]
		public string Server { get; set; } = "127.0.0.1";

		protected PIOServiceClient client;


		protected override void BeginProcessing()
		{
			base.BeginProcessing();

			Binding binding;
			EndpointAddress remoteAddress;

			binding = new BasicHttpBinding();
			remoteAddress = new EndpointAddress($@"http://{Server}:8733/Design_Time_Addresses/PIO.WebService/");
			client = new PIOServiceClient(binding, remoteAddress);
			client.Open();
		}

		protected override void EndProcessing()
		{
			// finalizing
			base.EndProcessing();
			client?.Close();
		}
		protected override void StopProcessing()
		{
			// abnormal
			base.StopProcessing();
			client?.Close();
		}


	}
}
