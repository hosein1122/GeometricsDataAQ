using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public interface BlocketteFactory
	{

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public Blockette parseBlockette(int type, byte[] bytes, boolean swapBytes) throws java.io.IOException, SeedFormatException;
		Blockette parseBlockette(int type, byte[] bytes, bool swapBytes);

	}

}
