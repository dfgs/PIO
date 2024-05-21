using StronglyTypedIds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	[StronglyTypedId(Template.Int)]
	public partial struct FactoryID	
	{
		public static FactoryID New() { return new FactoryID(UniqueIDGenerator<FactoryID>.GenerateID()); }
	}

	[StronglyTypedId(Template.Int)]
	public partial struct WorkerID
	{
		public static WorkerID New() { return new WorkerID(UniqueIDGenerator<WorkerID>.GenerateID()); }
	}

	[StronglyTypedId(Template.Int)]
	public partial struct ConnectorID
	{
		public static ConnectorID New() { return new ConnectorID(UniqueIDGenerator<ConnectorID>.GenerateID()); }
	}


	[StronglyTypedId(Template.Int)]
	public partial struct ConnectionID
	{
		public static ConnectionID New() { return new ConnectionID(UniqueIDGenerator<ConnectionID>.GenerateID()); }
	}

	[StronglyTypedId(Template.Int)]
	public partial struct BufferID
	{
		public static BufferID New() { return new BufferID(UniqueIDGenerator<BufferID>.GenerateID()); }
	}
	[StronglyTypedId(Template.Int)]
	public partial struct RecipeID
	{
		public static RecipeID New() { return new RecipeID(UniqueIDGenerator<RecipeID>.GenerateID()); }
	}
	[StronglyTypedId(Template.Int)]
	public partial struct IngredientID
	{
		public static IngredientID New() { return new IngredientID(UniqueIDGenerator<IngredientID>.GenerateID()); }
	}
	[StronglyTypedId(Template.Int)]
	public partial struct ProductID
	{
		public static ProductID New() { return new ProductID(UniqueIDGenerator<ProductID>.GenerateID()); }
	}
}
