using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	/// <summary>
	/// BlocketteUnknown.java
	/// 
	/// 
	/// Created: Mon Apr 5 15:48:51 1999
	/// 
	/// @author Philip Crotwell
	/// @version
	/// </summary>

	public class BlocketteUnknown : Blockette
	{

		public BlocketteUnknown(byte[] info, int type, bool swapBytes)
		{
			this.info = info;
			this.type = type;
			this.swapBytes = swapBytes;
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

		public virtual int calcSize()
		{
			byte[] lengthBytes = new byte[4];
			Array.Copy(info, 3, lengthBytes, 0, 4);
			return int.Parse(StringHelper.NewString(lengthBytes));
		}

		public override byte[] ToBytes()
		{
			return info;
		}

		public override void WriteASCII(TextWriter @out)
		{
			string infoStr = StringHelper.NewString(info);
			@out.WriteLine("Blockette " + Type + ": " + infoStr);
		}

		public virtual bool SwapBytes
		{
			get
			{
				return swapBytes;
			}
		}

		protected internal int type;

		protected internal byte[] info;

		protected internal bool swapBytes;
	} // BlocketteUnknown
}




