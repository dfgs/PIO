using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface IBufferManager : IPIOModule
	{
		IBuffer[]? GetBuffers();
		IBuffer? GetBuffer(BufferID BufferID);
		IBuffer? GetBuffer(ConnectorID ConnectorID);

		bool IsBufferValid(IBuffer Buffer);
		bool UpdateBuffer(IBuffer Buffer, float Cycle);
	}
}
