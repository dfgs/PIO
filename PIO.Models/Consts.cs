using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models
{
	public enum DataTest { One,Two,Three };

	public enum ResourceTypeIDs : int { Tree = 0, Wood = 1, Stone = 2, Coal = 3, Plank = 4 };
	public enum FactoryTypeIDs : int { Forest = 0, Stockpile = 1, Sawmill = 2 };
	public enum TaskTypeIDs : int { Idle=0, Produce = 1, MoveTo = 2, CarryTo = 3,CreateBuilding=4, Build = 5, Take = 6 };



}
