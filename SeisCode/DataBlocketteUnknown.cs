using System.IO;

namespace SeisCode
{
    public class DataBlocketteUnknown : DataBlockette
	{

		public DataBlocketteUnknown(byte[] info, int type, bool swapBytes) : base(info, swapBytes)
		{
			this.type = type;
		}

		public override int Type
		{
			get
			{
				return type;
			}
		}

		public override string Name
		{
			get
			{
				return "Unknown";
			}
		}

		public override int Size
		{
			get
			{
				return info.Length;
			}
		}

		public override byte[] ToBytes()
		{
			return info;
		}

		public override void WriteASCII(TextWriter @out)
		{
			@out.WriteLine("Blockette: " + Type);
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
