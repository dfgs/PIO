using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models
{

	public enum ResourceTypeIDs : int { Wood = 0,  Stone = 1, Plank = 3, CutStone=4, Coal = 5 };
	public enum BuildingTypeIDs : int { Forest = 0, Sawmill = 1, Stockpile = 2, StoneCutter = 3 };
	public enum TaskTypeIDs : int { Idle=0, Produce = 1, Harvest =2, MoveTo = 3, CreateBuilding=4, Build = 5, Take = 6, Store = 7 };



}
