using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class TopologySorter : ITopologySorter
	{
		public TopologySorter() 
		{ 
		}

		public IEnumerable<IFactory> Sort(IEnumerable<IFactory> Factories)
		{
			List<IFactory> sortedFactories;
			List<IFactory> factoriesToSort;
			List<IFactory> tempMarks;

			if (Factories == null) throw new PIOInvalidParameterException(nameof(Factories));

			sortedFactories = new List<IFactory>();
			factoriesToSort = new List<IFactory>();
			tempMarks = new List<IFactory>();

			factoriesToSort.AddRange(Factories);
			while(factoriesToSort.Count > 0)
			{
				Visit(factoriesToSort[0],tempMarks,factoriesToSort,sortedFactories);
			}

			return sortedFactories;
		}

		private void Visit(IFactory Factory, List<IFactory> TempMarks, List<IFactory> FactoriesToSort, List<IFactory> SortedFactories)
		{
			if (!FactoriesToSort.Contains(Factory)) return;
			if (TempMarks.Contains(Factory)) throw new InvalidOperationException("At least one cycle detected in factory's dependencies");
			
			TempMarks.Add(Factory);

			//foreach()
			//{
			// visit next factory
			//}

			TempMarks.Remove(Factory);
			FactoriesToSort.Remove(Factory);
			SortedFactories.Insert(0, Factory);
		}

	}
}
