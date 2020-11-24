using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	[Serializable]
	public abstract class DataBlockette : Blockette
	{

		public DataBlockette(byte[] info, bool swapBytes)
		{
			this.info = info;
			this.swapBytes = swapBytes;
		}

		public DataBlockette(int size)
		{
			this.info = new byte[size];
			Array.Copy(Utility.intToByteArray(Type), 2, info, 0, 2);
		}

		/// <summary>
		/// For use by subclasses that want to ensure that they are of a given size. </summary>
		/// <exception cref="SeedFormatException"> if the size is larger than the number of bytes </exception>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: protected void checkMinimumSize(int size) throws SeedFormatException
		protected internal virtual void checkMinimumSize(int size)
		{
			if (info.Length < size)
			{
				throw new SeedFormatException("Blockette " + Type + " must have at least " + size + " bytes, but got " + info.Length);
			}
		}

		/// <summary>
		/// For use by subclasses that want to ensure that they are of a given size. </summary>
		/// <exception cref="SeedFormatException"> if the size is larger than the number of bytes </exception>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: protected void trimToSize(int size) throws SeedFormatException
		protected internal virtual void trimToSize(int size)
		{
			checkMinimumSize(size);
			if (info.Length > size)
			{
				// must be extra junk at end, trim
				byte[] tmp = new byte[size];
				Array.Copy(info, 0, tmp, 0, size);
				info = tmp;
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void write(java.io.DataOutputStream dos, short nextOffset) throws java.io.IOException
		public virtual void write(BinaryWriter dos, short nextOffset)
		{
			dos.Write(toBytes(nextOffset));
		}

		public virtual byte[] toBytes(short nextOffset)
		{
			Array.Copy(Utility.intToByteArray(nextOffset), 2, info, 2, 2);
			return info;
		}

		public  override byte[] ToBytes()
		{
			return toBytes(0);
		}

		protected internal byte[] info;

		protected internal bool swapBytes;

	} // DataBlockette
}
