using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Ingredient
	{
		[DataMember]
		public int IngredientID { get; set; }
		[DataMember]
		public int FactoryTypeID { get; set; }
		[DataMember]
		public int ResourceTypeID { get; set; }
		[DataMember]
		public int Quantity { get; set; }


	}
}
