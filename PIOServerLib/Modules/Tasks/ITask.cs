using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Tasks
{
	public interface ITask
	{
		int TaskID
		{
			get;
		}

		void Leave();
		void Enter();
	}
}
