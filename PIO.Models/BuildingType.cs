﻿using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class BuildingType
	{
		[DataMember]
		public BuildingTypeIDs BuildingTypeID { get; set; }
		[DataMember]
		public string PhraseKey { get; set; }
		[DataMember]
		public int HealthPoints { get; set; }
		[DataMember]
		public int BuildSteps { get; set; }
		[DataMember]
		public bool IsFactory { get; set; }
		[DataMember]
		public bool IsFarm { get; set; }

	}
}
