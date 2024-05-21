using Microsoft.VisualBasic.FileIO;
using Moq;
using PIO.CoreLib;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib.UnitTests
{
	internal static class MockedData
	{
		public static IDataSource DataSource1;
		public static IDataSource DataSource2;
		private static ITopologySorter mainSorter;

		static MockedData()
		{
			mainSorter = new TopologySorter();

			DataSource1 = InitDataSource1();
			DataSource2 = InitDataSource2();
		}

		private static IDataSource InitDataSource1()
		{
			DataSource dataSource;
			IFactory factory1, factory2, factory3;
			IJob job1,job2,job3;	
			ISubTask subTask1,subTask2,subTask3;
			IRecipe recipe1, recipe2, recipe3;
			IIngredient ingredient1, ingredient2, ingredient3;
			IProduct product1, product2, product3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2;
			IBuffer buffer1, buffer2, buffer3, buffer4, buffer5, buffer6;

			// F1 -out1-in2-> F2 -out0.5-in2-> F3 -out0.25->

			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type2") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type3") };

			job1 = new Job() { ID = new JobID(1), FactoryID = new FactoryID(1) };
			job2 = new Job() { ID = new JobID(2), FactoryID = new FactoryID(2) };
			job3 = new Job() { ID = new JobID(3), FactoryID = new FactoryID(3) };

			subTask1 = new SubTask() { ID = new SubTaskID(1), JobID = new JobID(1) };
			subTask2 = new SubTask() { ID = new SubTaskID(2), JobID = new JobID(2) };
			subTask3 = new SubTask() { ID = new SubTaskID(3), JobID = new JobID(3) };

			recipe1 = new Recipe() { ID = new RecipeID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			recipe2 = new Recipe() { ID = new RecipeID(2), FactoryTypeID = new FactoryTypeID("Type2") };
			recipe3 = new Recipe() { ID = new RecipeID(3), FactoryTypeID = new FactoryTypeID("Type3") };

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 2 };
			ingredient3 = new Ingredient() { ID = new IngredientID(3), RecipeID = new RecipeID(3), ResourceType = "Type3", Rate = 2 };

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };
			product3 = new Product() { ID = new ProductID(3), RecipeID = new RecipeID(3), ResourceType = "Type4", Rate = 1 };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 1 };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type3" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(4), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(5), FactoryID = new FactoryID(2), ResourceType = "Type3" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(6), FactoryID = new FactoryID(3), ResourceType = "Type4" };

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(4), DestinationID = new ConnectorID(2) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(5), DestinationID = new ConnectorID(3) };

			buffer1 = new PIO.CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(1), Capacity = 100, InitialUsage = 0 };
			buffer2 = new PIO.CoreLib.Buffer() { ID = new BufferID(2), ConnectorID = new ConnectorID(2), Capacity = 100, InitialUsage = 0 };
			buffer3 = new PIO.CoreLib.Buffer() { ID = new BufferID(3), ConnectorID = new ConnectorID(3), Capacity = 100, InitialUsage = 0 };
			buffer4 = new PIO.CoreLib.Buffer() { ID = new BufferID(4), ConnectorID = new ConnectorID(4), Capacity = 100, InitialUsage = 0 };
			buffer5 = new PIO.CoreLib.Buffer() { ID = new BufferID(5), ConnectorID = new ConnectorID(5), Capacity = 100, InitialUsage = 0 };
			buffer6 = new PIO.CoreLib.Buffer() { ID = new BufferID(6), ConnectorID = new ConnectorID(6), Capacity = 100, InitialUsage = 0 };

			dataSource = new DataSource();

			dataSource.AddFactory(factory1);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory3);

			dataSource.AddJob(job1);
			dataSource.AddJob(job2);
			dataSource.AddJob(job3);

			dataSource.AddSubTask(subTask1);
			dataSource.AddSubTask(subTask2);
			dataSource.AddSubTask(subTask3);

			dataSource.AddRecipe(recipe1);
			dataSource.AddRecipe(recipe2);
			dataSource.AddRecipe(recipe3);

			dataSource.AddIngredient(ingredient1);
			dataSource.AddIngredient(ingredient2);
			dataSource.AddIngredient(ingredient3);

			dataSource.AddProduct(product1);
			dataSource.AddProduct(product2);
			dataSource.AddProduct(product3);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);

			dataSource.AddBuffer(buffer1);
			dataSource.AddBuffer(buffer2);
			dataSource.AddBuffer(buffer3);
			dataSource.AddBuffer(buffer4);
			dataSource.AddBuffer(buffer5);
			dataSource.AddBuffer(buffer6);

			return dataSource;
		}

		private static IDataSource InitDataSource2()
		{
			DataSource dataSource;
			IFactory factory1, factory2, factory3;
			IJob job1, job2, job3;
			ISubTask subTask1, subTask2, subTask3;
			IRecipe recipe1, recipe2, recipe3;
			IIngredient ingredient1, ingredient2;
			IProduct product1, product2, product3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3, inputConnector4;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2;
			IBuffer buffer1, buffer2, buffer3, buffer4, buffer5, buffer6, buffer7;


			// F1 -out1-in2-> F3 -out0.5->
			// F2 -out2-in2->  

			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type2") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type3") };

			job1 = new Job() { ID = new JobID(1), FactoryID = new FactoryID(1) };
			job2 = new Job() { ID = new JobID(2), FactoryID = new FactoryID(2) };
			job3 = new Job() { ID = new JobID(3), FactoryID = new FactoryID(3) };

			subTask1 = new SubTask() { ID = new SubTaskID(1), JobID = new JobID(1) };
			subTask2 = new SubTask() { ID = new SubTaskID(2), JobID = new JobID(2) };
			subTask3 = new SubTask() { ID = new SubTaskID(3), JobID = new JobID(3) };

			recipe1 = new Recipe() { ID = new RecipeID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			recipe2 = new Recipe() { ID = new RecipeID(2), FactoryTypeID = new FactoryTypeID("Type2") };
			recipe3 = new Recipe() { ID = new RecipeID(3), FactoryTypeID = new FactoryTypeID("Type3") };

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(3), ResourceType = "Type2", Rate = 2 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(3), ResourceType = "Type3", Rate = 2 };
				
			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 2 };
			product3 = new Product() { ID = new ProductID(3), RecipeID = new RecipeID(3), ResourceType = "Type4", Rate = 1 };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type2" };
			inputConnector4 = new InputConnector() { ID = new ConnectorID(4), FactoryID = new FactoryID(3), ResourceType = "Type3" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(5), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(6), FactoryID = new FactoryID(2), ResourceType = "Type3" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(7), FactoryID = new FactoryID(3), ResourceType = "Type4" };

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(5), DestinationID = new ConnectorID(3) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(6), DestinationID = new ConnectorID(4) };


			buffer1 = new PIO.CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(1), Capacity = 100, InitialUsage = 0 };
			buffer2 = new PIO.CoreLib.Buffer() { ID = new BufferID(2), ConnectorID = new ConnectorID(2), Capacity = 100, InitialUsage = 0 };
			buffer3 = new PIO.CoreLib.Buffer() { ID = new BufferID(3), ConnectorID = new ConnectorID(3), Capacity = 100, InitialUsage = 0 };
			buffer4 = new PIO.CoreLib.Buffer() { ID = new BufferID(4), ConnectorID = new ConnectorID(4), Capacity = 100, InitialUsage = 0 };
			buffer5 = new PIO.CoreLib.Buffer() { ID = new BufferID(5), ConnectorID = new ConnectorID(5), Capacity = 100, InitialUsage = 0 };
			buffer6 = new PIO.CoreLib.Buffer() { ID = new BufferID(6), ConnectorID = new ConnectorID(6), Capacity = 100, InitialUsage = 0 };
			buffer7 = new PIO.CoreLib.Buffer() { ID = new BufferID(7), ConnectorID = new ConnectorID(7), Capacity = 100, InitialUsage = 0 };


			dataSource = new DataSource();

			dataSource.AddFactory(factory1);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory3);

			dataSource.AddJob(job1);
			dataSource.AddJob(job2);
			dataSource.AddJob(job3);

			dataSource.AddSubTask(subTask1);
			dataSource.AddSubTask(subTask2);
			dataSource.AddSubTask(subTask3);

			dataSource.AddRecipe(recipe1);
			dataSource.AddRecipe(recipe2);
			dataSource.AddRecipe(recipe3);

			dataSource.AddIngredient(ingredient1);
			dataSource.AddIngredient(ingredient2);

			dataSource.AddProduct(product1);
			dataSource.AddProduct(product2);
			dataSource.AddProduct(product3);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);
			dataSource.AddInputConnector(inputConnector4);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);

			dataSource.AddBuffer(buffer1);
			dataSource.AddBuffer(buffer2);
			dataSource.AddBuffer(buffer3);
			dataSource.AddBuffer(buffer4);
			dataSource.AddBuffer(buffer5);
			dataSource.AddBuffer(buffer7);

			return dataSource;
		}


		public static IDataSource GetMockedDataSource(IDataSource Model)
		{
			IDataSource dataSource;

			dataSource = Mock.Of<IDataSource>();

			Mock.Get(dataSource).Setup(m => m.GetFactories()).Returns(Model.GetFactories());
			Mock.Get(dataSource).Setup(m => m.GetFactory(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetFactory(id));
			Mock.Get(dataSource).Setup(m => m.GetJobs(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetJobs(id));
			Mock.Get(dataSource).Setup(m => m.GetJob(It.IsAny<JobID>())).Returns<JobID>(id => Model.GetJob(id));
			Mock.Get(dataSource).Setup(m => m.GetSubTasks(It.IsAny<JobID>())).Returns<JobID>(id => Model.GetSubTasks(id));
			Mock.Get(dataSource).Setup(m => m.GetSubTasks()).Returns(Model.GetSubTasks());
			Mock.Get(dataSource).Setup(m => m.GetSubTask(It.IsAny<SubTaskID>())).Returns<SubTaskID>(id => Model.GetSubTask(id));
			Mock.Get(dataSource).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Returns<FactoryTypeID>(id => Model.GetRecipe(id));
			Mock.Get(dataSource).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetIngredients(id));
			Mock.Get(dataSource).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetProducts(id));
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetInputConnectors(id));
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetOutputConnectors(id));
			Mock.Get(dataSource).Setup(m => m.GetConnection(It.IsAny<ConnectionID>())).Returns<ConnectionID>(id => Model.GetConnection(id));
			Mock.Get(dataSource).Setup(m => m.GetConnections(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetConnections(id));
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetInputConnector(id));
			Mock.Get(dataSource).Setup(m => m.GetOutputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetOutputConnector(id));
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Returns(Model.GetBuffers());
			Mock.Get(dataSource).Setup(m => m.GetBuffer(It.IsAny<BufferID>())).Returns<BufferID>(id => Model.GetBuffer(id));
			Mock.Get(dataSource).Setup(m => m.GetBuffer(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetBuffer(id));

			return dataSource;
		}
		public static IFactoryManager GetMockedFactoryManager(IDataSource Model)
		{
			IFactoryManager factoryManager;

			factoryManager = Mock.Of<IFactoryManager>();
			Mock.Get(factoryManager).Setup(m => m.GetFactories()).Returns(Model.GetFactories().ToArray());
			Mock.Get(factoryManager).Setup(m => m.GetFactory(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetFactory(id));

			return factoryManager;
		}
		public static ISubTaskManager GetMockedSubTaskManager(IDataSource Model)
		{
			ISubTaskManager subTaskManager;

			subTaskManager = Mock.Of<ISubTaskManager>();
			Mock.Get(subTaskManager).Setup(m => m.GetSubTasks(It.IsAny<JobID>())).Returns<JobID>(id => Model.GetSubTasks(id).ToArray());
			Mock.Get(subTaskManager).Setup(m => m.GetSubTasks()).Returns(Model.GetSubTasks().ToArray());
			Mock.Get(subTaskManager).Setup(m => m.GetSubTask(It.IsAny<SubTaskID>())).Returns<SubTaskID>(id => Model.GetSubTask(id));

			return subTaskManager;
		}

		public static IRecipeManager GetMockedRecipeManager(IDataSource Model)
		{
			IRecipeManager recipeManager;

			recipeManager = Mock.Of<IRecipeManager>();
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetIngredients(id).ToArray());
			Mock.Get(recipeManager).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetProducts(id).ToArray());
			Mock.Get(recipeManager).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Returns<FactoryTypeID>(id => Model.GetRecipe(id));

			return recipeManager;
		}

		public static IConnectionManager GetMockedConnectionManager(IDataSource Model)
		{
			IConnectionManager connectionManager;

			connectionManager = Mock.Of<IConnectionManager>();
			Mock.Get(connectionManager).Setup(m => m.GetInputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetInputConnector(id));
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetInputConnectors(id).ToArray());
			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetOutputConnectors(id).ToArray());
			Mock.Get(connectionManager).Setup(m => m.GetConnections(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetConnections(id).ToArray());

			return connectionManager;
		}

		public static IBufferManager GetMockedBufferManager(IDataSource Model)
		{
			IBufferManager bufferManager;

			bufferManager = Mock.Of<IBufferManager>();
			Mock.Get(bufferManager).Setup(m => m.GetBuffers()).Returns(Model.GetBuffers().ToArray());
			Mock.Get(bufferManager).Setup(m => m.GetBuffer(It.IsAny<BufferID>())).Returns<BufferID>(id => Model.GetBuffer(id));
			Mock.Get(bufferManager).Setup(m => m.GetBuffer(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetBuffer(id));
			Mock.Get(bufferManager).Setup(m => m.IsBufferValid(It.IsAny<IBuffer>())).Returns<IBuffer>(buffer => buffer.IsValid);
			Mock.Get(bufferManager).Setup(m => m.UpdateBuffer(It.IsAny<IBuffer>(), It.IsAny<float>())).Returns<IBuffer,float>((buffer ,cycle)=> { buffer.Update(cycle);return true; });

			return bufferManager;
		}


		public static ITopologySorter GetMockedTopologySorter()
		{
			ITopologySorter mock;

			mock = Mock.Of<ITopologySorter>();
			Mock.Get(mock).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns<IDataSource>(dataSource=>mainSorter.Sort(dataSource));

			return mock;
		}
	}

}
