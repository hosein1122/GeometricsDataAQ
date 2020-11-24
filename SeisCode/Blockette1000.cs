using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	/// <summary>
	/// Blockette1000.java
	/// 
	/// 
	/// Created: Fri Apr  2 14:51:42 1999
	/// 
	/// @author Philip Crotwell
	/// @version
	/// </summary>

	public class Blockette1000 : DataBlockette, RecordLengthBlockette
	{

		public const int B1000_SIZE = 8;

		public const sbyte SEED_BIG_ENDIAN = 1;

		public const sbyte SEED_LITTLE_ENDIAN = 0;

		public Blockette1000() : base(B1000_SIZE)
		{
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public Blockette1000(byte[] info, boolean swapBytes) throws SeedFormatException
		public Blockette1000(sbyte[] info, bool swapBytes) : base(info, swapBytes)
		{
			trimToSize(B1000_SIZE);
		}

		public virtual int Size
		{
			get
			{
				return B1000_SIZE;
			}
		}

		public virtual int Type
		{
			get
			{
				return 1000;
			}
		}

		public virtual string Name
		{
			get
			{
				return "Data Only SEED Blockette";
			}
		}

		/// <summary>
		/// Get the value of encodingFormat. </summary>
		/// <returns> Value of encodingFormat. </returns>
		public virtual sbyte EncodingFormat
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


		/// <summary>
		/// Get the value of wordOrder. </summary>
		/// <returns> Value of wordOrder. </returns>
		public virtual sbyte WordOrder
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


		public virtual bool BigEndian
		{
			get
			{
				if (info[5] == 1)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public virtual bool LittleEndian
		{
			get
			{
				return !BigEndian;
			}
		}

		public virtual int LogicalRecordLengthByte
		{
			get
			{
				return DataRecordLengthByte;
			}
		}

		public virtual int LogicalRecordLength
		{
			get
			{
				return getDataRecordLength();
			}
		}

		/// <summary>
		/// Get the value of dataRecordLengthByte. </summary>
		/// <returns> Value of dataRecordLengthByte. </returns>
		public virtual sbyte DataRecordLengthByte
		{
			get
			{
				return info[6];
			}
		}

		/// <summary>
		/// Get the value of dataRecordLengthByte. </summary>
		/// <returns> Value of dataRecordLengthByte. </returns>
		public virtual int getDataRecordLength()
		{
			if (DataRecordLengthByte < 31)
			{
				return (0x01 << DataRecordLengthByte);
			}
			else
			{
				throw new Exception("Data Record Length exceeds size of int");
			}
		}

		/// <summary>
		/// Set the value of dataRecordLength. </summary>
		/// <param name="v">  Value to assign to dataRecordLength. </param>
		public virtual void setDataRecordLength(sbyte v)
		{
			info[6] = v;
		}

		/// <summary>
		/// Get the value of reserved. </summary>
		/// <returns> Value of reserved. </returns>
		public virtual sbyte Reserved
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


		public virtual void writeASCII(PrintWriter @out)
		{
			@out.println("Blockette1000 encod=" + EncodingFormat + " wOrder=" + WordOrder + " recLen=" + DataRecordLengthByte);
		}

		public override string ToString()
		{
			return base.ToString() + "  format=" + EncodingFormat;
		}

	} // Blockette1000

}
