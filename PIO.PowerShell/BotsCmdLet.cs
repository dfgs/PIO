using PIO.Bots.ClientLib.BotsServiceReference;
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
	public class BotsCmdLet: BaseCmdLet<BotsServiceClient>
	{
		protected override BotsServiceClient OnCreateClient()
		{
			Binding binding;
			EndpointAddress remoteAddress;
			BotsServiceClient client;

			binding = new BasicHttpBinding();
			remoteAddress = new EndpointAddress($@"http://{Server}:8734/PIO/Bots/Service/");
			client=new BotsServiceClient(binding, remoteAddress);
			client.Open();

			return client;
		}

		protected override void OnCloseClient()
		{
			client?.Close();
		}

		

	}
}
