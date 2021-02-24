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
		ProduceOrder GetProduceOrder(int ProduceOrderID);
		[OperationContract]
		ProduceOrder[] GetProduceOrders(int PlanetID);

		[OperationContract]
		HarvestOrder GetHarvestOrder(int HarvestOrderID);
		[OperationContract]
		HarvestOrder[] GetHarvestOrders(int PlanetID);
	
		[OperationContract]
		BuildOrder GetBuildOrder(int BuildOrderID);
		[OperationContract]
		BuildOrder[] GetBuildOrders(int PlanetID);
	

		#endregion

		#region functional
		[OperationContract]
		ProduceOrder CreateProduceOrder(int PlanetID, int BuildingID);
		[OperationContract]
		HarvestOrder CreateHarvestOrder(int PlanetID, int BuildingID);
		[OperationContract]
		BuildOrder CreateBuildOrder(int PlanetID, BuildingTypeIDs BuildingTypeID, int X, int Y);
		
		[OperationContract]
		Bot CreateBot(int WorkerID);
		[OperationContract]
		void DeleteBot(int BotID);
		#endregion

	}


}
