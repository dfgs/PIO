﻿using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class TaskType
	{
		[DataMember]
		public TaskTypeIDs TaskTypeID { get; set; }
		[DataMember]
		public string PhraseKey { get; set; }
		

	}
}
