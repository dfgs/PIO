using NetORMLib.Databases;
using NetORMLib.Tables;
using PIO.ServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib
{
	public static class PIODB
	{
		public static PlanetTable PlanetTable = new PlanetTable();
		public static CellTable CellTable = new CellTable();
		public static BuildingTable BuildingTable = new BuildingTable();
		public static FactoryTable FactoryTable = new FactoryTable();
		public static FactoryTypeTable FactoryTypeTable = new FactoryTypeTable();
		public static FarmTable FarmTable = new FarmTable();
		public static FarmTypeTable FarmTypeTable = new FarmTypeTable();
		public static IngredientTable IngredientTable = new IngredientTable();
		public static MaterialTable MaterialTable = new MaterialTable();
		public static ProductTable ProductTable = new ProductTable();
		public static ResourceTypeTable ResourceTypeTable = new ResourceTypeTable();
		public static StackTable StackTable = new StackTable();
		public static TaskTable TaskTable = new TaskTable();
		public static TaskTypeTable TaskTypeTable = new TaskTypeTable();
		public static WorkerTable WorkerTable = new WorkerTable();


	}
}
