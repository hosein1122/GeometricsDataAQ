using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	/// <summary>
	/// Utility.java
	/// 
	/// 
	/// Created: Fri Apr 2 14:28:55 1999
	/// 
	/// @author Philip Crotwell
	/// </summary>
	public class Utility
	{

		public static int extractInteger(byte[] info, int start, int length)
		{
			return int.Parse(extractString(info, start, length));
		}

		public static string extractString(byte[] info, int start, int length)
		{
			byte[] subbytes = new byte[length];
			Array.Copy(info, start, subbytes, 0, length);
			return StringHelper.NewString(subbytes);
		}

		public static string extractVarString(byte[] info, int start, int maxLength)
		{
			return extractTermString(info, start, maxLength, (byte)126);
		}

		public static string extractNullTermString(byte[] info, int start, int maxLength)
		{
			return extractTermString(info, start, maxLength, (byte)0);
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public static void writeNullTermString(String value, int maxLength, java.io.DataOutput out) throws java.io.IOException
		public static void writeNullTermString(string value, int maxLength, BinaryWriter @out)
		{
			string s = value;
			if (s.Length > maxLength)
			{
				s = s.Substring(0, maxLength);
			}
			@out.Write(s);
			for (int i = s.Length; i < maxLength; i++)
			{
				@out.Write((byte)0);
			}
		}

		internal static string extractTermString(byte[] info, int start, int maxLength, byte termChar)
		{
			int length = 0;
			while (length < maxLength && start + length < info.Length && info[start + length] != termChar)
			{
				length++;
			}
			if (length == 0)
			{
				return "";
			}
			byte[] tmp = new byte[length];
			Array.Copy(info, start, tmp, 0, length);
			return StringHelper.NewString(tmp);
		}

		public static short bytesToShort(byte hi, byte low, bool swapBytes)
		{
			if (swapBytes)
			{
				return (short)((hi & 0xff) + ((low & 0xff) << 8));
			}
			else
			{
				return (short)(((hi & 0xff) << 8) + (low & 0xff));
			}
		}

		public static int bytesToInt(byte a)
		{
			return (int)a;
		}

		public static int uBytesToInt(byte a)
		{
			// we and with 0xff in order to get the sign correct (pos)
			return a & 0xff;
		}

		public static int bytesToInt(byte[] info, int start, bool swapBytes)
		{
			return bytesToInt(info[start], info[start + 1], info[start + 2], info[start + 3], swapBytes);
		}

		public static long bytesToLong(byte[] info, int start, bool swapBytes)
		{
			return bytesToLong(info[start], info[start + 1], info[start + 2], info[start + 3], info[start + 4], info[start + 5], info[start + 6], info[start + 7], swapBytes);
		}

		public static int bytesToInt(byte a, byte b, bool swapBytes)
		{
			if (swapBytes)
			{
				return (a & 0xff) + ((int)b << 8);
			}
			else
			{
				return ((int)a << 8) + (b & 0xff);
			}
		}

		public static int uBytesToInt(byte a, byte b, bool swapBytes)
		{
			// we "and" with 0xff to get the sign correct (pos)
			if (swapBytes)
			{
				return (a & 0xff) + ((b & 0xff) << 8);
			}
			else
			{
				return ((a & 0xff) << 8) + (b & 0xff);
			}
		}

		public static int bytesToInt(byte a, byte b, byte c, bool swapBytes)
		{
			if (swapBytes)
			{
				return (a & 0xff) + ((b & 0xff) << 8) + ((int)c << 16);
			}
			else
			{
				return ((int)a << 16) + ((b & 0xff) << 8) + (c & 0xff);
			}
		}

		public static int bytesToInt(byte a, byte b, byte c, byte d, bool swapBytes)
		{
			if (swapBytes)
			{
				return ((a & 0xff)) + ((b & 0xff) << 8) + ((c & 0xff) << 16) + ((d & 0xff) << 24);
			}
			else
			{
				return ((a & 0xff) << 24) + ((b & 0xff) << 16) + ((c & 0xff) << 8) + ((d & 0xff));
			}
		}

		public static long bytesToLong(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h, bool swapBytes)
		{
			if (swapBytes)
			{
				return ((a & 0xffl)) + ((b & 0xffl) << 8) + ((c & 0xffl) << 16) + ((d & 0xffl) << 24) + ((e & 0xffl) << 32) + ((f & 0xffl) << 40) + ((g & 0xffl) << 48) + ((h & 0xffl) << 56);
			}
			else
			{
				return ((a & 0xffl) << 56) + ((b & 0xffl) << 48) + ((c & 0xffl) << 40) + ((d & 0xffl) << 32) + ((e & 0xffl) << 24) + ((f & 0xffl) << 16) + ((g & 0xffl) << 8) + ((h & 0xffl));
			}
		}

		public static float bytesToFloat(byte a, byte b, byte c, byte d, bool swapBytes)
		{
			return bytesToInt(a, b, c, d, swapBytes);
		}

		public static double bytesToDouble(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h, bool swapBytes)
		{
			//return Double.longBitsToDouble(bytesToLong(a, b, c, d, e, f, g, h, swapBytes));
			return bytesToLong(a, b, c, d, e, f, g, h, swapBytes);
		}

		public static double bytesToDouble(byte[] info, int start, bool swapBytes)
		{
			//return Double.longBitsToDouble(Utility.bytesToLong(info, start, swapBytes));
			return bytesToLong(info, start, swapBytes);
		}

		public static float bytesToFloat(byte[] info, int start, bool swapBytes)
		{
			return Utility.bytesToInt(info, start, swapBytes);
		}

		public static byte[] intToByteArray(int a)
		{
			byte[] returnByteArray = new byte[4]; // int is 4 bytes
			returnByteArray[0] = unchecked((byte)((a >> 24) & 0xff));
			returnByteArray[1] = unchecked((byte)((a >> 16) & 0xff));
			returnByteArray[2] = unchecked((byte)((a >> 8) & 0xff));
			returnByteArray[3] = unchecked((byte)((a) & 0xff));
			return returnByteArray;
		}

		public static byte[] floatToByteArray(float a)
		{
			//return intToByteArray(Float.floatToIntBits(a));
			return intToByteArray((int)a);
		}


		public static byte[] longToByteArray(long a)
		{
			byte[] returnByteArray = new byte[8]; // long is 8 bytes
			returnByteArray[0] = unchecked((byte)(((long)((ulong)a >> 56)) & 0xffl));
			returnByteArray[1] = unchecked((byte)(((long)((ulong)a >> 48)) & 0xffl));
			returnByteArray[2] = unchecked((byte)(((long)((ulong)a >> 40)) & 0xffl));
			returnByteArray[3] = unchecked((byte)(((long)((ulong)a >> 32)) & 0xffl));
			returnByteArray[4] = unchecked((byte)(((long)((ulong)a >> 24)) & 0xffl));
			returnByteArray[5] = unchecked((byte)(((long)((ulong)a >> 16)) & 0xffl));
			returnByteArray[6] = unchecked((byte)(((long)((ulong)a >> 8)) & 0xffl));
			returnByteArray[7] = unchecked((byte)((a) & 0xffl));
			return returnByteArray;
		}

		public static byte[] doubleToByteArray(double d)
		{
			return longToByteArray(System.BitConverter.DoubleToInt64Bits(d));
		}
		/// <summary>
		/// Inserts float into dest at index pos 
		/// </summary>
		public static void insertFloat(float value, byte[] dest, int pos)
		{
			int bits = (int)value;
			byte[] b = Utility.intToByteArray(bits);
			Array.Copy(b, 0, dest, pos, 4);
		}

		public static byte[] pad(byte[] source, int requiredBytes, byte paddingByte)
		{
			if (source.Length == requiredBytes)
			{
				return source;
			}
			else
			{
				byte[] returnByteArray = new byte[requiredBytes];
				Array.Copy(source, 0, returnByteArray, 0, source.Length);
				for (int i = source.Length; i < requiredBytes; i++)
				{
					returnByteArray[i] = paddingByte;
				}
				return returnByteArray;
			}
		}

		public static byte[] format(byte[] source, int start, int end)
		{
			byte[] returnByteArray = new byte[start - end + 1];
			int j = 0;
			for (int i = start; i < end; i++, j++)
			{
				returnByteArray[j] = source[i];
			}
			return returnByteArray;
		}

		public static bool areContiguous(DataRecord first, DataRecord second)
		{
			Btime fEnd = first.PredictedNextStartBtime;
			Btime sBegin = second.Header.StartBtime;
			return fEnd.tenthMilli == sBegin.tenthMilli && fEnd.sec == sBegin.sec && fEnd.min == sBegin.min && fEnd.hour == sBegin.hour && fEnd.jday == sBegin.jday && fEnd.year == sBegin.year;
		}

		/// <summary>
		/// breaks the List into sublists where the DataRecords are contiguous. Assumes
		/// that the input List is sorted (by begin time?) and does not contain overlaps.
		/// </summary>
		public static IList<IList<DataRecord>> breakContiguous(IList<DataRecord> inList)
		{
			IList<IList<DataRecord>> @out = new List<IList<DataRecord>>();
			IList<DataRecord> subout = new List<DataRecord>();
			DataRecord prev = null;
			foreach (DataRecord dataRecord in inList)
			{
				if (prev == null)
				{
					// first one
					@out.Add(subout);
				}
				else if (areContiguous(prev, dataRecord))
				{
					// contiguous
				}
				else
				{
					subout = new List<DataRecord>();
					@out.Add(subout);
				}
				subout.Add(dataRecord);
				prev = dataRecord;
			}
			return @out;
		}

		public static void Main(string[] args)
		{
			int a = 256;
			byte a1 = (byte)((a & 0xff000000) >> 24);
			byte a2 = (byte)((a & 0x00ff0000) >> 16);
			byte a3 = (byte)((a & 0x0000ff00) >> 8);
			byte a4 = unchecked((byte)((a & 0x000000ff)));
			Console.WriteLine("first byte is " + a1);
			Console.WriteLine("2 byte is " + a2);
			Console.WriteLine("3 byte is " + a3);
			Console.WriteLine("4  byte is " + a4);
			byte[] source = new byte[5];
			for (int i = 0; i < 5; i++)
			{
				source[i] = (byte)10;
			}
			byte[] output = Utility.pad(source, 5, (byte)32);
			// for(int j = 0; j< output.length; j++)
			// {
			// System.out.println("byte"+j+" " + output[j]);
			// }
			for (int k = output.Length - 1; k > -1; k--)
			{
				Console.WriteLine("byte" + k + " " + output[k]);
			}
		}

		public static void cleanDuplicatesOverlaps(IList<DataRecord> drFromFileList)
		{
			//drFromFileList.Sort(new DataRecordBeginComparator());
			var comparator = new DataRecordBeginComparator();
			drFromFileList.Sort((x, y) => comparator.compare(x, y));
			DataRecord prev = null;
			IEnumerator<DataRecord> itFromFileList = drFromFileList.GetEnumerator();
			while (itFromFileList.MoveNext())
			{
				DataRecord dataRecord = itFromFileList.Current;
				if (prev != null && prev.Header.StartBtime.Equals(dataRecord.Header.StartBtime))
				{
					//  a duplicate
					//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
					itFromFileList.remove();
				}
				else if (prev != null && prev.LastSampleBtime.afterOrEquals(dataRecord.Header.StartBtime))
				{
					//  a overlap
					//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
					itFromFileList.remove();
				}
				else
				{
					prev = dataRecord;
				}
			}
		}
	} // Utility

}
