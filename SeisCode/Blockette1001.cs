using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class Blockette1001 : DataBlockette
	{

		public const int B1001_SIZE = 8;

		public Blockette1001() : base(B1001_SIZE)
		{
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public Blockette1001(byte[] info, boolean swapBytes) throws SeedFormatException
		public Blockette1001(sbyte[] info, bool swapBytes) : base(info, swapBytes)
		{
			trimToSize(B1001_SIZE);
		}

		public virtual int Size
		{
			get
			{
				return B1001_SIZE;
			}
		}

		public virtual int Type
		{
			get
			{
				return 1001;
			}
		}

		public virtual string Name
		{
			get
			{
				return "Data Extension Blockette";
			}
		}
		public virtual sbyte TimingQuality
		{
			get
			{
				return info[4];
			}
			set
			{
				info[4] = value;
			}
		}


		public virtual sbyte Microsecond
		{
			get
			{
				return info[5];
			}
			set
			{
				info[5] = value;
			}
		}


		public virtual sbyte Reserved
		{
			get
			{
				return info[6];
			}
			set
			{
				info[6] = value;
			}
		}


		public virtual sbyte FrameCount
		{
			get
			{
				return info[7];
			}
			set
			{
				info[7] = value;
			}
		}


		public override void writeASCII(PrintWriter @out)
		{
			@out.println("Blockette1001 tQual=" + TimingQuality + " microsec=" + Microsecond + " frameC=" + FrameCount);
		}


	}

}
