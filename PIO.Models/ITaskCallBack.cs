using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models
{
	public interface ITaskCallBack
	{
		[OperationContract(IsOneWay = true)]
		System.Threading.Tasks.Task OnTaskStarted(Task Task);
		[OperationContract(IsOneWay = true)]
		System.Threading.Tasks.Task OnTaskEnded(Task Task);
	}
}
