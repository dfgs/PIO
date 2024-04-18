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
			IRecipe? recipe=null;

			LogEnter();

			Log(LogLevels.Information, "Sorting factories by dependency");
			if (!Try(() => topologySorter.Sort(DataSource)).Then(items => sortedFactories = items.ToArray()).OrAlert("Failed to sort factories")) return false;

			Log(LogLevels.Information, "Updating all factories");
			foreach(IFactory factory in sortedFactories)
			{
				if (!Try(() => DataSource.GetRecipe(factory.FactoryType)).Then(result => recipe = result).OrAlert($"Failed to get recipe for factory with ID {factory.ID}")) continue;
				if (recipe == null)
				{
					Log(LogLevels.Warning, $"No recipe found for factory with ID {factory.ID}");
					continue;
				}

			}

			return true;
		}



	}
}
