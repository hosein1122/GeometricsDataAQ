using System.IO;

namespace SeisCode
{
    public class Blockette1001 : DataBlockette
	{

		public const int B1001_SIZE = 8;

		public Blockette1001() : base(B1001_SIZE)
		{
		}

		public Blockette1001(byte[] info, bool swapBytes) : base(info, swapBytes)
		{
			TrimToSize(B1001_SIZE);
		}

		public override int Size
		{
			get
			{
				return B1001_SIZE;
			}
		}

		public override int Type
		{
			get
			{
				return 1001;
			}
		}

		public override string Name
		{
			get
			{
				return "Data Extension Blockette";
			}
		}
		public virtual byte TimingQuality
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


		public virtual byte Microsecond
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


		public virtual byte Reserved
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


		public virtual byte FrameCount
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


		public override void WriteASCII(TextWriter @out)
		{
			@out.WriteLine("Blockette1001 tQual=" + TimingQuality + " microsec=" + Microsecond + " frameC=" + FrameCount);
		}


	}

}
