﻿using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface IJobManager:IPIOModule
	{
		IJob[]? GetJobs(FactoryID FactoryID);
		IJob? GetJob(JobID JobID);


	}
}
