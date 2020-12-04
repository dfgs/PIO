using LogLib;
using PIO.BotsLib.Basic;
using PIO.ClientLib.PIOServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIO.BotsConsole
{
	class Program
	{
		private static AutoResetEvent quitEvent;

		static void Main(string[] args)
		{
			ILogger logger;

			PIOServiceClient client;
			Binding binding;
			EndpointAddress remoteAddress;

			IdleBot bot;

			quitEvent = new AutoResetEvent(false);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

			//logger = new ConsoleLogger(new DefaultLogFormatter());
			logger = new UnicastLogger(IPAddress.Loopback, Properties.Settings.Default.UnicastPort);


			binding = new BasicHttpBinding();
			remoteAddress = new EndpointAddress($@"http://{Properties.Settings.Default.PIOServerAddress}:8733/Design_Time_Addresses/PIO.WebService/");
			client = new PIOServiceClient(binding, remoteAddress);
			client.Open();


			bot = new IdleBot(logger, client, 1, 10);
			bot.Start();

			WaitHandle.WaitAny(new WaitHandle[] { quitEvent }, -1);

			bot.Stop();
			client.Close();
			logger.Dispose();

			Console.CancelKeyPress -= new ConsoleCancelEventHandler(Console_CancelKeyPress);

		}


		static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			if (e.SpecialKey == ConsoleSpecialKey.ControlC)
			{
				Console.WriteLine("Control break invoked");
				e.Cancel = true;
				quitEvent.Set();
			}

		}


	}
}
