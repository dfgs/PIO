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

}
