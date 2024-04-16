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

		public IEnumerable<IFactory> Sort(IDataSource DataSource)
		{
			List<IFactory> sortedFactories;
			List<IFactory> factoriesToSort;
			List<IFactory> tempMarks;

			if (DataSource == null) throw new PIOInvalidParameterException(nameof(DataSource));

			sortedFactories = new List<IFactory>();
			factoriesToSort = new List<IFactory>();
			tempMarks = new List<IFactory>();

			factoriesToSort.AddRange(DataSource.GetFactories());
			while(factoriesToSort.Count > 0)
			{
				Visit(DataSource, factoriesToSort[0],tempMarks,factoriesToSort,sortedFactories);
			}

			return sortedFactories;
		}

		private void Visit(IDataSource DataSource, IFactory Factory, List<IFactory> TempMarks, List<IFactory> FactoriesToSort, List<IFactory> SortedFactories)
		{
			IInputConnector? nextConnector;
			IFactory? nextFactory;

			if (!FactoriesToSort.Contains(Factory)) return;
			if (TempMarks.Contains(Factory)) throw new InvalidOperationException("At least one cycle detected in factory's dependencies");
			
			TempMarks.Add(Factory);

			foreach(IOutputConnector outputConnector in DataSource.GetOutputConnectors(Factory.ID))
			{
				foreach(IConnection connection in DataSource.GetConnections(outputConnector.ID))
				{
					nextConnector = DataSource.GetInputConnector(connection.DestinationID);
					if (nextConnector == null) continue;
					nextFactory = DataSource.GetFactory(nextConnector.FactoryID);
					if (nextFactory == null) continue;
					Visit(DataSource,nextFactory,TempMarks,FactoriesToSort,SortedFactories);
				}
			}

			TempMarks.Remove(Factory);
			FactoriesToSort.Remove(Factory);
			SortedFactories.Add(Factory);
		}

	}
}
