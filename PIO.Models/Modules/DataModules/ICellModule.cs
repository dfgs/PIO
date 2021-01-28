using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface ICellModule : IDatabaseModule
	{
		Cell GetCell(int CellID);
		Cell GetCell(int PlanetID, int X,int Y);
		Cell[] GetCells(int PlanetID, int X, int Y,int Width,int Height);

		
		
	}
}
