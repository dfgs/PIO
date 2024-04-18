using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;

namespace PIO.ModulesLib
{
	public class FactoryManager : Module,IFactoryManager
	{
		private ITopologySorter topologySorter;

		public FactoryManager(ILogger Logger,ITopologySorter TopologySorter) : base(Logger)
		{
			if (TopologySorter==null) throw new PIOInvalidParameterException(nameof(TopologySorter));
			this.topologySorter = TopologySorter;
		}

		public bool Update(IDataSource DataSource, float Cycle)
		{
			IFactory[] sortedFactories = [];

			LogEnter();

			Log(LogLevels.Information, "Sorting factories by dependency");
			if (!Try(() => topologySorter.Sort(DataSource)).Then(items => sortedFactories = items.ToArray()).OrAlert("Failed to sort factories")) return false;

			Log(LogLevels.Information, "Updating all factories");
			foreach(IFactory factory in sortedFactories)
			{
				

			}

			return true;
		}



	}
}
