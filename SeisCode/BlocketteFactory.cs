using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public interface BlocketteFactory
	{
		Blockette ParseBlockette(int type, byte[] bytes, bool swapBytes);

	}

}
