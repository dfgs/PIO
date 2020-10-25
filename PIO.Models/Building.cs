using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Building
	{
		[DataMember]
		public int BuildingID { get; set; }
		[DataMember]
		public int PlanetID { get; set; }
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public int Y { get; set; }
		[DataMember]
		public BuildingTypeIDs BuildingTypeID { get; set; }
		[DataMember]
		public int HealthPoints { get; set; }
		[DataMember]
		public int RemainingBuildSteps { get; set; }


	}
}
