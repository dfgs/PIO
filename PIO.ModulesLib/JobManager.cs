using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class JobManager : PIOModule,IJobManager
	{
		
		public JobManager(ILogger Logger,IDataSource DataSource) : base(Logger,DataSource)
		{
		}

		public IJob[]? GetJobs(FactoryID FactoryID)
		{
			IJob[]? jobs = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Factory ID {FactoryID}] Trying to get jobs");
			if (!Try(() => DataSource.GetJobs(FactoryID)).Then(result=>jobs=result.ToArray()).OrAlert($"[Factory ID {FactoryID}] Failed to get jobs")) return null;
			return jobs;
		}
	

		public IJob? GetJob(JobID JobID)
		{
			IJob? job = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Job ID {JobID}] Trying to get job");
			if (!Try(() => DataSource.GetJob(JobID)).Then(result => job = result).OrAlert($"[Job ID {JobID}] Failed to get job")) return null;
			return job;

		}

		


	}
}
