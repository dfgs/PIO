using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models
{

	public enum ResourceTypeIDs : int { Tree = 0, Wood = 1, Stone = 2, Coal = 3, Plank = 4 };
	public enum BuildingTypeIDs : int { Forest = 0, Stockpile = 1, Sawmill = 2, Stone = 3, Water = 4 };
	public enum TaskTypeIDs : int { Idle=0, Produce = 1, MoveTo = 2, CreateBuilding=3, Build = 4, Take = 5, Store = 6 };



}
