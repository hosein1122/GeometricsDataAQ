using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class DefaultBlocketteFactory : BlocketteFactory
	{

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public Blockette parseBlockette(int type, byte[] bytes, boolean swapBytes) throws java.io.IOException, SeedFormatException
		public virtual Blockette parseBlockette(int type, sbyte[] bytes, bool swapBytes)
		{
			switch (type)
			{
				case 5:
					return new Blockette5(bytes);
				case 8:
					return new Blockette8(bytes);
				case 10:
					return new Blockette10(bytes);
				case 100:
					return new Blockette100(bytes, swapBytes);
				case 200:
					return new Blockette200(bytes, swapBytes);
				case 1000:
					return new Blockette1000(bytes, swapBytes);
				case 1001:
					return new Blockette1001(bytes, swapBytes);
				case 2000:
					return new Blockette2000(bytes, swapBytes);
				default:
					if (type < 100)
					{
						return new BlocketteUnknown(bytes, type, swapBytes);
					}
					else
					{
						return new DataBlocketteUnknown(bytes, type, swapBytes);
					}
			}
		}
	}

}
