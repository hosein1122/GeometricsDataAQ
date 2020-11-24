using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class DataBlocketteUnknown : DataBlockette
	{

		public DataBlocketteUnknown(sbyte[] info, int type, bool swapBytes) : base(info, swapBytes)
		{
			this.type = type;
		}

		public virtual int Type
		{
			get
			{
				return type;
			}
		}

		public virtual string Name
		{
			get
			{
				return "Unknown";
			}
		}

		public virtual int Size
		{
			get
			{
				return info.length;
			}
		}

		public virtual sbyte[] toBytes()
		{
			return info;
		}

		public virtual void writeASCII(PrintWriter @out)
		{
			@out.println("Blockette: " + Type);
		}

		public virtual bool SwapBytes
		{
			get
			{
				return swapBytes;
			}
		}

		protected internal int type;

	}

}
