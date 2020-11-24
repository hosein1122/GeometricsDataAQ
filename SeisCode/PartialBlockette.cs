using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	public class PartialBlockette : BlocketteUnknown
	{

		public PartialBlockette(int type, byte[] info, bool swapBytes, int priorBytes, int totalBytes) : base(info, type, swapBytes)
		{
			bytesRead = info.Length;
			this.priorBytes = priorBytes;
			this.totalBytes = totalBytes;
		}

		public static PartialBlockette combine(PartialBlockette first, PartialBlockette second)
		{
			byte[] tmp = new byte[first.Size + second.Size];
			Array.Copy(first.ToBytes(), 0, tmp, 0, first.Size);
			Array.Copy(second.ToBytes(), 0, tmp, first.Size, second.Size);
			return new PartialBlockette(first.Type, tmp, first.swapBytes, first.PriorSize, first.TotalSize);
		}

		public virtual void writeASCII(TextWriter @out)
		{
			string infoStr = Encoding.ASCII.GetString(info);
			@out.WriteLine("Partial Blockette " + Type + ", " + bytesRead + " with " + priorBytes + " prior of " + totalBytes + " total bytes: " + infoStr);
		}

		public virtual bool Begin
		{
			get
			{
				return priorBytes == 0;
			}
		}

		public virtual bool End
		{
			get
			{
				return priorBytes + bytesRead == totalBytes;
			}
		}

		public virtual int TotalSize
		{
			get
			{
				return totalBytes;
			}
		}

		public virtual int PriorSize
		{
			get
			{
				return priorBytes;
			}
		}

		public virtual int SoFarSize
		{
			get
			{
				return priorBytes + bytesRead;
			}
		}

		internal int totalBytes;

		internal int priorBytes;

		internal int bytesRead;

		public virtual int BytesRead
		{
			get
			{
				return bytesRead;
			}
		}
	}

}
