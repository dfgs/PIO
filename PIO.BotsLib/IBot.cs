﻿using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.BotsLib
{
	public interface IBot
	{
		int WorkerID
		{
			get;
		}
		PIO.Models.Task RunTask();
		PIO.Models.Task GetCurrentTask();

	}
}
