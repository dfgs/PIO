using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class TopologySorterUnitTest
	{
		[TestMethod]
		public void SortShouldThrowExceptionIfParameterIsNull()
		{
			ITopologySorter sorter;

			sorter=new TopologySorter();
			#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => sorter.Sort(null));
			#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void SortShouldThrowAnExceptionIfCycleIsDetected()
		{
			ITopologySorter sorter;
			IDataSource dataSource;
			//IFactory[] results;

			IFactory factory1, factory2, factory3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2, connection3;


			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type1") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type1") };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };

			// F1 -> F2 -> F3 -> F1
			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(2) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(2), DestinationID = new ConnectorID(3) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(3), DestinationID = new ConnectorID(1) };

			dataSource = new DataSource();
			
			dataSource.AddFactory(factory1);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory3);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			dataSource.AddConnection(connection3);

			sorter = new TopologySorter();
			Assert.ThrowsException<PIOInvalidOperationException>(()=>sorter.Sort(dataSource), "At least one cycle detected in factory's dependencies");
			
		}

		[TestMethod]
		public void SortShouldReturnValidValuesCase1()
		{
			ITopologySorter sorter;
			IDataSource dataSource;
			IFactory[] results;

			IFactory factory1, factory2, factory3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2, connection3;


			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type1") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type1") };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };
			
			// F1 -> F2 -> F3
			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(2) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(2), DestinationID = new ConnectorID(3) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(3), DestinationID = new ConnectorID(1) };

			dataSource = new DataSource();

			dataSource.AddFactory(factory3);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory1);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			//dataSource.AddConnection(connection3);

			sorter = new TopologySorter();
			results = sorter.Sort(dataSource).ToArray();

			Assert.AreEqual(3, results.Length);
			Assert.AreEqual(factory1.ID, results[0].ID);
			Assert.AreEqual(factory2.ID, results[1].ID);
			Assert.AreEqual(factory3.ID, results[2].ID);

		}

		[TestMethod]
		public void SortShouldReturnValidValuesCase2()
		{
			ITopologySorter sorter;
			IDataSource dataSource;
			IFactory[] results;

			IFactory factory1, factory2, factory3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2, connection3;


			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type1") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type1") };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };

			// F1 -> F2
			//    -> F3
			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(2) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(3), DestinationID = new ConnectorID(1) };

			dataSource = new DataSource();

			dataSource.AddFactory(factory3);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory1);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			//dataSource.AddConnection(connection3);

			sorter = new TopologySorter();
			results = sorter.Sort(dataSource).ToArray();

			Assert.AreEqual(3, results.Length);
			Assert.AreEqual(factory1.ID, results[0].ID);
			Assert.AreEqual(factory2.ID, results[1].ID);
			Assert.AreEqual(factory3.ID, results[2].ID);

		}

		[TestMethod]
		public void SortShouldReturnValidValuesCase3()
		{
			ITopologySorter sorter;
			IDataSource dataSource;
			IFactory[] results;

			IFactory factory1, factory2, factory3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2, connection3;


			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type1") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type1") };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };

			// F1 -> F3
			// F2 -> F3
			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(3), DestinationID = new ConnectorID(1) };

			dataSource = new DataSource();

			dataSource.AddFactory(factory3);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory1);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			//dataSource.AddConnection(connection3);

			sorter = new TopologySorter();
			results = sorter.Sort(dataSource).ToArray();

			Assert.AreEqual(3, results.Length);
			Assert.AreEqual(factory1.ID, results[0].ID);
			Assert.AreEqual(factory2.ID, results[1].ID);
			Assert.AreEqual(factory3.ID, results[2].ID);

		}

		[TestMethod]
		public void SortShouldReturnValidValuesCase4()
		{
			ITopologySorter sorter;
			IDataSource dataSource;
			IFactory[] results;

			IFactory factory1, factory2, factory3, factory4;
			IInputConnector inputConnector1, inputConnector2, inputConnector3, inputConnector4;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3, outputConnector4;
			IConnection connection1, connection2, connection3;


			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type1") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type1") };
			factory4 = new Factory() { ID = new FactoryID(4), FactoryTypeID = new FactoryTypeID("Type1") };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };
			inputConnector4 = new InputConnector() { ID = new ConnectorID(4), FactoryID = new FactoryID(4), ResourceType = "Type1" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type1" };
			outputConnector4 = new OutputConnector() { ID = new ConnectorID(4), FactoryID = new FactoryID(4), ResourceType = "Type1" };

			// F1 -> F3 -> F4
			// F2 ->    -> F4
			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(2), DestinationID = new ConnectorID(4) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(3), DestinationID = new ConnectorID(4) };

			dataSource = new DataSource();

			dataSource.AddFactory(factory3);
			dataSource.AddFactory(factory4);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory1);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);
			dataSource.AddInputConnector(inputConnector4);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);
			dataSource.AddOutputConnector(outputConnector4);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			dataSource.AddConnection(connection3);

			sorter = new TopologySorter();
			results = sorter.Sort(dataSource).ToArray();

			Assert.AreEqual(4, results.Length);
			Assert.AreEqual(factory1.ID, results[0].ID);
			Assert.AreEqual(factory2.ID, results[1].ID);
			Assert.AreEqual(factory3.ID, results[2].ID);
			Assert.AreEqual(factory4.ID, results[3].ID);

		}


	}

}