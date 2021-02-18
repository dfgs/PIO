using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.Models
{
	[DataContract]
	public class BuildFarmOrder:Order
	{
		[DataMember]
		public int BuildFarmOrderID { get; set; }
		[DataMember]
		public BuildingTypeIDs BuildingTypeID { get; set; }
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public int Y { get; set; }
	}

}
