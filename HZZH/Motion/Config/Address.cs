using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motion
{
	public enum Address : ushort
	{
		X = 0,
		Y = 1,
		Z = 2,
		R = 3,
		AxLZ = 4,
		AxLBoxBelt = 5,
		AxNeedleCheck = 7,
		AxFrameAdjust = 8,
		AxFrameBelt = 9,
		AxUZ = 10,
		AxUBoxBelt = 11,
	}
}
