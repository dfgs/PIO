using PIO.Bots.Models;
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
		Order GetOrder(int OrderID);
		[OperationContract]
		Order[] GetOrders();
		[OperationContract]
		ProduceOrder GetProduceOrder(int ProduceOrderID);
		[OperationContract]
		ProduceOrder[] GetProduceOrders();
		#endregion


	}


}
