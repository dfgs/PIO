using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IDataSource
	{
		void AddFactory(IFactory Factory);
		void AddInputConnector(IInputConnector Connector);
		void AddOutputConnector(IOutputConnector Connector);
		void AddConnection(IConnection Connection);
		void AddBuffer(IBuffer Buffer);
		void AddRecipe(IRecipe Recipe);
		void AddIngredient(IIngredient Ingredient);
		void AddProduct(IProduct Product);

		IFactory? GetFactory(FactoryID FactoryID);
		IEnumerable<IFactory> GetFactories();
		
		IInputConnector? GetInputConnector(ConnectorID ConnectorID);
		IEnumerable<IInputConnector> GetInputConnectors(FactoryID FactoryID);

		IOutputConnector? GetOutputConnector(ConnectorID ConnectorID);
		IEnumerable<IOutputConnector> GetOutputConnectors(FactoryID FactoryID);



		IConnection? GetConnection(ConnectionID ConnectionID);
		IEnumerable<IConnection> GetConnections(ConnectorID SourceConnectorID);

		IEnumerable<IBuffer> GetBuffers();
		IBuffer? GetBuffer(BufferID BufferID);
		IBuffer? GetBuffer(ConnectorID ConnectorID);

		IRecipe? GetRecipe(RecipeID RecipeID);
		IRecipe? GetRecipe(FactoryTypeID FactoryTypeID);

		IIngredient? GetIngredient(IngredientID IngredientID);
		IEnumerable<IIngredient> GetIngredients(RecipeID RecipeID);

		IProduct? GetProduct(ProductID ProductID);
		IEnumerable<IProduct> GetProducts(RecipeID RecipeID);

	}
}
