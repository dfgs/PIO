using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models
{
	[DataContract]
	public class Phrase
	{
		[DataMember]
		public int PhraseID { get; set; }
		[DataMember]
		public string Key { get; set; }
		[DataMember]
		public string CountryCode { get; set; }
		[DataMember]
		public string Value { get; set; }
		
		public static int GenerateID(string Key,string CountryCode)
		{
			return $"{Key}_{CountryCode}".GetHashCode();
		}
	}
}
