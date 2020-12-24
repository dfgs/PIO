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
	public class PIOCmdLet: BaseCmdLet<PIOServiceClient>
	{
		protected override PIOServiceClient OnCreateClient()
		{
			Binding binding;
			EndpointAddress remoteAddress;
			PIOServiceClient client;

			binding = new BasicHttpBinding();
			remoteAddress = new EndpointAddress($@"http://{Server}:8733/PIO/Service/");
			client=new PIOServiceClient(binding, remoteAddress);
			client.Open();

			return client;
		}

		protected override void OnCloseClient()
		{
			client?.Close();
		}

		

	}
}
