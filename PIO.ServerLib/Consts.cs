using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib
{
	public enum ResourceTypeIDs : int { Tree = 0, Wood = 1, Stone = 2, Coal = 3, Plank = 4 };
	public enum FactoryTypeIDs : int { Forest = 0, Stockpile = 1, WoodCutter = 2 };
	public enum TaskTypeIDs : int { Produce = 0, MoveTo=1 };


}
