using System;
using System.Collections.Generic;
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

		public static int extractInteger(sbyte[] info, int start, int length)
		{
			return int.Parse(extractString(info, start, length));
		}

		public static string extractString(sbyte[] info, int start, int length)
		{
			sbyte[] subbytes = new sbyte[length];
			Array.Copy(info, start, subbytes, 0, length);
			return StringHelper.NewString(subbytes);
		}

		public static string extractVarString(sbyte[] info, int start, int maxLength)
		{
			return extractTermString(info, start, maxLength, (sbyte)126);
		}

		public static string extractNullTermString(sbyte[] info, int start, int maxLength)
		{
			return extractTermString(info, start, maxLength, (sbyte)0);
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public static void writeNullTermString(String value, int maxLength, java.io.DataOutput out) throws java.io.IOException
		public static void writeNullTermString(string value, int maxLength, DataOutput @out)
		{
			string s = value;
			if (s.Length > maxLength)
			{
				s = s.Substring(0, maxLength);
			}
			@out.writeBytes(s);
			for (int i = s.Length; i < maxLength; i++)
			{
				@out.write((sbyte)0);
			}
		}

		internal static string extractTermString(sbyte[] info, int start, int maxLength, sbyte termChar)
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
			sbyte[] tmp = new sbyte[length];
			Array.Copy(info, start, tmp, 0, length);
			return StringHelper.NewString(tmp);
		}

		public static short bytesToShort(sbyte hi, sbyte low, bool swapBytes)
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

		public static int bytesToInt(sbyte a)
		{
			return (int)a;
		}

		public static int uBytesToInt(sbyte a)
		{
			// we and with 0xff in order to get the sign correct (pos)
			return a & 0xff;
		}

		public static int bytesToInt(sbyte[] info, int start, bool swapBytes)
		{
			return bytesToInt(info[start], info[start + 1], info[start + 2], info[start + 3], swapBytes);
		}

		public static long bytesToLong(sbyte[] info, int start, bool swapBytes)
		{
			return bytesToLong(info[start], info[start + 1], info[start + 2], info[start + 3], info[start + 4], info[start + 5], info[start + 6], info[start + 7], swapBytes);
		}

		public static int bytesToInt(sbyte a, sbyte b, bool swapBytes)
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

		public static int uBytesToInt(sbyte a, sbyte b, bool swapBytes)
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

		public static int bytesToInt(sbyte a, sbyte b, sbyte c, bool swapBytes)
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

		public static int bytesToInt(sbyte a, sbyte b, sbyte c, sbyte d, bool swapBytes)
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

		public static long bytesToLong(sbyte a, sbyte b, sbyte c, sbyte d, sbyte e, sbyte f, sbyte g, sbyte h, bool swapBytes)
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

		public static float bytesToFloat(sbyte a, sbyte b, sbyte c, sbyte d, bool swapBytes)
		{
			return Float.intBitsToFloat(bytesToInt(a, b, c, d, swapBytes));
		}

		public static double bytesToDouble(sbyte a, sbyte b, sbyte c, sbyte d, sbyte e, sbyte f, sbyte g, sbyte h, bool swapBytes)
		{
			return Double.longBitsToDouble(bytesToLong(a, b, c, d, e, f, g, h, swapBytes));
		}

		public static double bytesToDouble(sbyte[] info, int start, bool swapBytes)
		{
			return Double.longBitsToDouble(Utility.bytesToLong(info, start, swapBytes));
		}

		public static float bytesToFloat(sbyte[] info, int start, bool swapBytes)
		{
			return Float.intBitsToFloat(Utility.bytesToInt(info, start, swapBytes));
		}

		public static sbyte[] intToByteArray(int a)
		{
			sbyte[] returnByteArray = new sbyte[4]; // int is 4 bytes
			returnByteArray[0] = unchecked((sbyte)((a >> 24) & 0xff));
			returnByteArray[1] = unchecked((sbyte)((a >> 16) & 0xff));
			returnByteArray[2] = unchecked((sbyte)((a >> 8) & 0xff));
			returnByteArray[3] = unchecked((sbyte)((a) & 0xff));
			return returnByteArray;
		}

		public static sbyte[] floatToByteArray(float a)
		{
			return intToByteArray(Float.floatToIntBits(a));
		}


		public static sbyte[] longToByteArray(long a)
		{
			sbyte[] returnByteArray = new sbyte[8]; // long is 8 bytes
			returnByteArray[0] = unchecked((sbyte)(((long)((ulong)a >> 56)) & 0xffl));
			returnByteArray[1] = unchecked((sbyte)(((long)((ulong)a >> 48)) & 0xffl));
			returnByteArray[2] = unchecked((sbyte)(((long)((ulong)a >> 40)) & 0xffl));
			returnByteArray[3] = unchecked((sbyte)(((long)((ulong)a >> 32)) & 0xffl));
			returnByteArray[4] = unchecked((sbyte)(((long)((ulong)a >> 24)) & 0xffl));
			returnByteArray[5] = unchecked((sbyte)(((long)((ulong)a >> 16)) & 0xffl));
			returnByteArray[6] = unchecked((sbyte)(((long)((ulong)a >> 8)) & 0xffl));
			returnByteArray[7] = unchecked((sbyte)((a) & 0xffl));
			return returnByteArray;
		}

		public static sbyte[] doubleToByteArray(double d)
		{
			return longToByteArray(System.BitConverter.DoubleToInt64Bits(d));
		}
		/// <summary>
		/// Inserts float into dest at index pos 
		/// </summary>
		public static void insertFloat(float value, sbyte[] dest, int pos)
		{
			int bits = Float.floatToIntBits(value);
			sbyte[] b = Utility.intToByteArray(bits);
			Array.Copy(b, 0, dest, pos, 4);
		}

		public static sbyte[] pad(sbyte[] source, int requiredBytes, sbyte paddingByte)
		{
			if (source.Length == requiredBytes)
			{
				return source;
			}
			else
			{
				sbyte[] returnByteArray = new sbyte[requiredBytes];
				Array.Copy(source, 0, returnByteArray, 0, source.Length);
				for (int i = source.Length; i < requiredBytes; i++)
				{
					returnByteArray[i] = paddingByte;
				}
				return returnByteArray;
			}
		}

		public static sbyte[] format(sbyte[] source, int start, int end)
		{
			sbyte[] returnByteArray = new sbyte[start - end + 1];
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
			sbyte a1 = (sbyte)((a & 0xff000000) >> 24);
			sbyte a2 = (sbyte)((a & 0x00ff0000) >> 16);
			sbyte a3 = (sbyte)((a & 0x0000ff00) >> 8);
			sbyte a4 = unchecked((sbyte)((a & 0x000000ff)));
			Console.WriteLine("first byte is " + a1);
			Console.WriteLine("2 byte is " + a2);
			Console.WriteLine("3 byte is " + a3);
			Console.WriteLine("4  byte is " + a4);
			sbyte[] source = new sbyte[5];
			for (int i = 0; i < 5; i++)
			{
				source[i] = (sbyte)10;
			}
			sbyte[] output = Utility.pad(source, 5, (sbyte)32);
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
			drFromFileList.Sort(new DataRecordBeginComparator());
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
