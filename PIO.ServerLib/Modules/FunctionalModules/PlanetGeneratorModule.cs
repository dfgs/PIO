using LogLib;
using PIO.Models;
using PIO.Models.Modules;
using PIOBaseModulesLib.Modules.FunctionalModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public class PlanetGeneratorModule : FunctionalModule,IPlanetGeneratorModule
	{
		private IPhraseModule phraseModule;
		private IResourceTypeModule resourceTypeModule;
		private IBuildingTypeModule buildingTypeModule;
		private ITaskTypeModule taskTypeModule;
		private IMaterialModule materialModule;
		private IIngredientModule ingredientModule;
		private IProductModule productModule;
		private IPlanetModule planetModule;
		private ICellModule cellModule;
		private IBuildingModule buildingModule;
		private IWorkerModule workerModule;

		public PlanetGeneratorModule(ILogger Logger,IPhraseModule PhraseModule, IResourceTypeModule ResourceTypeModule,IBuildingTypeModule BuildingTypeModule,ITaskTypeModule TaskTypeModule,IMaterialModule MaterialModule,IIngredientModule IngredientModule,IProductModule ProductModule,
			IPlanetModule PlanetModule,ICellModule CellModule,IBuildingModule BuildingModule,IWorkerModule WorkerModule
			) : base(Logger)
		{
			this.phraseModule = PhraseModule;
			this.resourceTypeModule = ResourceTypeModule;this.buildingTypeModule = BuildingTypeModule;this.taskTypeModule = TaskTypeModule;this.materialModule = MaterialModule;this.ingredientModule = IngredientModule;this.productModule = ProductModule;
			this.planetModule = PlanetModule;this.cellModule = CellModule;this.buildingModule = BuildingModule;this.workerModule = WorkerModule;
		}

		protected void OnGenerate()
		{
			Planet planet;
			Building building;
			Worker worker;
			
			Log(LogLevels.Information, "Creating Phrase items");
			phraseModule.CreatePhrase("Wood", "EN", "Wood");
			phraseModule.CreatePhrase("Wood", "FR", "Bois");


			Log(LogLevels.Information, "Creating ResourceType items");
			resourceTypeModule.CreateResourceType(Models.ResourceTypeIDs.Wood, "Wood");
			resourceTypeModule.CreateResourceType(Models.ResourceTypeIDs.Stone, "Stone");
			resourceTypeModule.CreateResourceType(Models.ResourceTypeIDs.Coal, "Coal");
			resourceTypeModule.CreateResourceType(Models.ResourceTypeIDs.Plank, "Plank");
			resourceTypeModule.CreateResourceType(Models.ResourceTypeIDs.CutStone, "CutStone");

			Log(LogLevels.Information, "Creating BuildingType items");
			buildingTypeModule.CreateBuildingType(Models.BuildingTypeIDs.Forest, "Forest", 1, 10, false, true);
			buildingTypeModule.CreateBuildingType(Models.BuildingTypeIDs.Stockpile, "Stockpile", 2, 10, false, false); ;
			buildingTypeModule.CreateBuildingType(Models.BuildingTypeIDs.Sawmill, "Sawmill", 2, 10, true, false);
			buildingTypeModule.CreateBuildingType(Models.BuildingTypeIDs.StoneCutter, "StoneCutter", 5, 10, true, false);

			Log(LogLevels.Information, "Creating TaskType items");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.Idle, "Idle");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.Produce, "Produce");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.Harvest, "Harvest");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.MoveTo, "Move to");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.CreateBuilding, "Create building");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.Build, "Build");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.Take, "Take");
			taskTypeModule.CreateTaskType(Models.TaskTypeIDs.Store, "Store");

			Log(LogLevels.Information, "Creating Material items");
			materialModule.CreateMaterial(Models.BuildingTypeIDs.Forest, Models.ResourceTypeIDs.Wood, 0);
			materialModule.CreateMaterial(Models.BuildingTypeIDs.Sawmill, Models.ResourceTypeIDs.Wood, 1);
			materialModule.CreateMaterial(Models.BuildingTypeIDs.StoneCutter, Models.ResourceTypeIDs.Plank, 1);
			materialModule.CreateMaterial(Models.BuildingTypeIDs.StoneCutter, Models.ResourceTypeIDs.Stone, 1);

			Log(LogLevels.Information, "Creating Ingredient items");
			ingredientModule.CreateIngredient(Models.BuildingTypeIDs.Sawmill, Models.ResourceTypeIDs.Wood, 1);
			ingredientModule.CreateIngredient(Models.BuildingTypeIDs.StoneCutter, Models.ResourceTypeIDs.Stone, 1);

			Log(LogLevels.Information, "Creating Product items");
			productModule.CreateProduct(Models.BuildingTypeIDs.Forest, Models.ResourceTypeIDs.Wood, 1, 10);
			productModule.CreateProduct(Models.BuildingTypeIDs.Sawmill, Models.ResourceTypeIDs.Plank, 2, 20);
			productModule.CreateProduct(Models.BuildingTypeIDs.StoneCutter, Models.ResourceTypeIDs.CutStone, 2, 20);

			Log(LogLevels.Information, "Creating Planet items");
			planet = planetModule.CreatePlanet("Default", 50, 50);


			Log(LogLevels.Information, "Creating Cell items");
			for (int x = 0; x < planet.Width; x++)
			{
				for (int y = 0; y < planet.Height; y++)
				{
					cellModule.CreateCell(planet.PlanetID, x, y);
				}
			}

			Log(LogLevels.Information, "Creating Building items");
			building = buildingModule.CreateBuilding(planet.PlanetID, 0, 0, BuildingTypeIDs.Forest, 0, 10);

			building = buildingModule.CreateBuilding(planet.PlanetID, 2, 2, BuildingTypeIDs.Sawmill, 0, 10);

			#region create startup Worker
			worker = workerModule.CreateWorker(planet.PlanetID, 0, 3);
			worker = workerModule.CreateWorker(planet.PlanetID, 2, 3);
			#endregion
		}
		public bool Generate()
		{

			LogEnter();
			return Try(() => OnGenerate()).OrAlert("Error occured during planet generation");
		}

	}
}
