﻿using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Worker : ILocation
	{
		[DataMember]
		public int WorkerID { get; set; }
		[DataMember]
		public int PlanetID { get; set; }
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public int Y { get; set; }

		[DataMember]
		public ResourceTypeIDs? ResourceTypeID { get; set; }




	}
}
