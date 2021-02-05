using PIO.Bots.Models;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PIO.Bots.Models
{
	[ServiceContract]
	public interface IBotsService
	{
		#region data
		[OperationContract]
		Bot GetBot(int BotID);
		[OperationContract]
		Bot GetBotForWorker(int WorkerID);
		[OperationContract]
		Bot[] GetBots();


		[OperationContract]
		Order GetOrder(int OrderID);
		[OperationContract]
		Order[] GetOrders();
		[OperationContract]
		ProduceOrder GetProduceOrder(int ProduceOrderID);
		[OperationContract]
		ProduceOrder[] GetProduceOrders();
		[OperationContract]
		ProduceOrder[] GetProduceOrdersForFactory(int FactoryID);
		[OperationContract]
		BuildFactoryOrder GetBuildFactoryOrder(int BuildFactoryOrderID);
		[OperationContract]
		BuildFactoryOrder[] GetBuildFactoryOrders();
		#endregion

		#region functional
		[OperationContract]
		ProduceOrder CreateProduceOrder(int PlanetID, int FactoryID);
		[OperationContract]
		BuildFactoryOrder CreateBuildFactoryOrder(int PlanetID, FactoryTypeIDs FactoryTypeID,int X,int Y);
		[OperationContract]
		Bot CreateBot(int WorkerID);
		#endregion

	}


}
