using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebServiceLib
{
    [DataContract]
    public class PIOFault
    {
        [DataMember]
        public string Message
		{
            get;
            set;
		}

        public PIOFault(string Message)
        {
            this.Message = Message;
        }

        
    }
}
