﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IPIOData<TId>
	{
		TId ID
		{
			get;
		}
	}
}
