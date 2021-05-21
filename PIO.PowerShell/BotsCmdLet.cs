
using PIO.Bots.ClientLib;
using PIO.ClientLib.PIOServiceReference;
using RESTLib.Client;
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
	public class BotsCmdLet: BaseCmdLet<BotsRESTClient>
	{
		protected override BotsRESTClient OnCreateClient()
		{
			BotsRESTClient client;

			client=new BotsRESTClient($@"http://{Server}:8734",new HttpConnector(),new ResponseDeserializer());

			return client;
		}

		protected override void OnCloseClient()
		{
			//client?.Close();
		}

		

	}
}
