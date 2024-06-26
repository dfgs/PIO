﻿using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class DataSource : IDataSource
	{
		private List<IFactory> factories;
		private List<IWorker> workers;
		private List<IJob> jobs;
		private List<ISubTask> subTasks;
		private List<IInputConnector> inputConnectors;
		private List<IOutputConnector> outputConnectors;
		private List<IConnection> connections;
		private List<IBuffer> buffers;
		private List<IRecipe> recipes;
		private List<IIngredient> ingredients;
		private List<IProduct> products;

		public DataSource() 
		{ 
			this.factories = new List<IFactory>();
			this.workers = new List<IWorker>();
			this.jobs = new List<IJob>();
			this.subTasks = new List<ISubTask>();
			this.inputConnectors = new List<IInputConnector>();
			this.outputConnectors = new List<IOutputConnector>();
			this.connections = new List<IConnection>();
			this.buffers = new List<IBuffer>();
			this.recipes = new List<IRecipe>();
			this.ingredients= new List<IIngredient>();
			this.products= new List<IProduct>();
		}

		public void AddFactory(IFactory Factory)
		{
			if (Factory == null) throw new PIOInvalidParameterException(nameof(Factory));
			this.factories.Add(Factory);
		}
		public void AddWorker(IWorker Worker)
		{
			if (Worker == null) throw new PIOInvalidParameterException(nameof(Worker));
			this.workers.Add(Worker);
		}
		public void AddJob(IJob Job)
		{
			if (Job == null) throw new PIOInvalidParameterException(nameof(Job));
			this.jobs.Add(Job);
		}
		public void AddSubTask(ISubTask SubTask)
		{
			if (SubTask == null) throw new PIOInvalidParameterException(nameof(SubTask));
			this.subTasks.Add(SubTask);
		}

		public void AddInputConnector(IInputConnector InputConnector)
		{
			if (InputConnector == null) throw new PIOInvalidParameterException(nameof(InputConnector));
			this.inputConnectors.Add(InputConnector);
		}
		public void AddOutputConnector(IOutputConnector OutputConnector)
		{
			if (OutputConnector == null) throw new PIOInvalidParameterException(nameof(OutputConnector));
			this.outputConnectors.Add(OutputConnector);
		}
		public void AddConnection(IConnection Connection)
		{
			if (Connection == null) throw new PIOInvalidParameterException(nameof(Connection));
			this.connections.Add(Connection); 
		}
		public void AddBuffer(IBuffer Buffer)
		{
			if (Buffer == null) throw new PIOInvalidParameterException(nameof(Buffer));
			this.buffers.Add(Buffer); 
		}
		public void AddRecipe(IRecipe Recipe)
		{
			if (Recipe == null) throw new PIOInvalidParameterException(nameof(Recipe));
			this.recipes.Add(Recipe);
		}
		public void AddIngredient(IIngredient Ingredient)
		{
			if (Ingredient == null) throw new PIOInvalidParameterException(nameof(Ingredient));
			this.ingredients.Add(Ingredient);
		}
		public void AddProduct(IProduct Product)
		{
			if (Product == null) throw new PIOInvalidParameterException(nameof(Product));
			this.products.Add(Product);
		}




		public IFactory? GetFactory(FactoryID FactoryID)
		{
			return factories.FirstOrDefault(item => item.ID == FactoryID);
		}
		public IEnumerable<IFactory> GetFactories()
		{
			return factories;
		}

		public IWorker? GetWorker(WorkerID WorkerID)
		{
			return workers.FirstOrDefault(item => item.ID == WorkerID);
		}
		public IEnumerable<IWorker> GetWorkers()
		{
			return workers;
		}
		public IJob? GetJob(JobID JobID)
		{
			return jobs.FirstOrDefault(item => item.ID == JobID);
		}

		public IEnumerable<IJob> GetJobs(FactoryID FactoryID)
		{
			return jobs.Where(item => item.FactoryID == FactoryID);
		}

		public ISubTask? GetSubTask(SubTaskID SubTaskID)
		{
			return subTasks.FirstOrDefault(item => item.ID == SubTaskID);
		}

		public IEnumerable<ISubTask> GetSubTasks(JobID JobID)
		{
			return subTasks.Where(item => item.JobID == JobID);
		}

		public IEnumerable<ISubTask> GetSubTasks()
		{
			return subTasks;
		}


		public IInputConnector? GetInputConnector(ConnectorID ConnectorID)
		{
			return inputConnectors.FirstOrDefault(item => item.ID == ConnectorID);
		}

		public IEnumerable<IInputConnector> GetInputConnectors(FactoryID FactoryID)
		{
			return inputConnectors.Where(item => item.FactoryID == FactoryID);
		}

		public IOutputConnector? GetOutputConnector(ConnectorID ConnectorID)
		{
			return outputConnectors.FirstOrDefault(item => item.ID == ConnectorID);
		}

		public IEnumerable<IOutputConnector> GetOutputConnectors(FactoryID FactoryID)
		{
			return outputConnectors.Where(item => item.FactoryID == FactoryID);
		}


		public IConnection? GetConnection(ConnectionID ConnectionID)
		{
			return connections.FirstOrDefault(item=>item.ID == ConnectionID);
		}

		public IEnumerable<IConnection> GetConnections(ConnectorID SourceConnectorID)
		{
			return connections.Where(item => item.SourceID == SourceConnectorID);
		}

		public IBuffer? GetBuffer(BufferID BufferID)
		{
			return buffers.FirstOrDefault(item=>item.ID==BufferID);
		}
		public IBuffer? GetBuffer(ConnectorID ConnectorID)
		{
			return buffers.FirstOrDefault(item=>item.ConnectorID==ConnectorID);
		}
		public IEnumerable<IBuffer> GetBuffers()
		{
			return buffers;
		}

		public IRecipe? GetRecipe(RecipeID RecipeID)
		{
			return recipes.FirstOrDefault(item => item.ID == RecipeID);
		}
		public IRecipe? GetRecipe(FactoryTypeID FactoryTypeID)
		{
			return recipes.FirstOrDefault(item => item.FactoryTypeID == FactoryTypeID);
		}

		public IIngredient? GetIngredient(IngredientID IngredientID)
		{
			return ingredients.FirstOrDefault(item => item.ID == IngredientID);
		}
		public IEnumerable<IIngredient> GetIngredients(RecipeID RecipeID)
		{
			return ingredients.Where(item => item.RecipeID == RecipeID);
		}

		public IProduct? GetProduct(ProductID ProductID)
		{
			return products.FirstOrDefault(item => item.ID == ProductID);
		}
		public IEnumerable<IProduct> GetProducts(RecipeID RecipeID)
		{
			return products.Where(item => item.RecipeID == RecipeID);
		}
	}
}
