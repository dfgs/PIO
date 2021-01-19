using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PIO.Models
{
	[ServiceContract(SessionMode = SessionMode.Required,CallbackContract = typeof(ITaskCallBack))]
	public interface ITaskCallbackService
	{

        [OperationContract(IsOneWay = false, IsInitiating = true)]
        bool Subscribe();
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        bool Unsubscribe();
        /*[OperationContract(IsOneWay = true)]
        void TaskStarted(Task Task);
        [OperationContract(IsOneWay = true)]
        void TaskEnded(Task Task);//*/
    }


}
