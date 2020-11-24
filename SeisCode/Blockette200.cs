using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class Blockette200 : DataBlockette
	{

		public Blockette200(float signal, float period, float background, Btime signalOnset, string eventDetector) : base(B200_SIZE)
		{
			Utility.insertFloat(signal, info, SIGNAL);
			Utility.insertFloat(period, info, PERIOD);
			Utility.insertFloat(background, info, BACKGROUND);
			sbyte[] onsetBytes = signalOnset.AsBytes;
			Array.Copy(onsetBytes, 0, info, SIGNAL_ONSET, onsetBytes.Length);
			if (eventDetector.Length > EVENT_DETECTOR_LENGTH)
			{
				throw new System.ArgumentException("The event detector can only be up to " + EVENT_DETECTOR_LENGTH + " characters in length");
			}
			sbyte[] detectorBytes;
			try
			{
				detectorBytes = eventDetector.GetBytes(Encoding.ASCII);
			}
			catch (UnsupportedEncodingException)
			{
				throw new Exception("Java was unable to find the US-ASCII character encoding.");
			}
			if (detectorBytes.Length != eventDetector.Length)
			{
				throw new System.ArgumentException("The characters in event detector must be in the ASCII character set i.e. from 0-127");
			}
			detectorBytes = Utility.pad(detectorBytes, EVENT_DETECTOR_LENGTH, (sbyte)' ');
			Array.Copy(detectorBytes, 0, info, EVENT_DETECTOR, detectorBytes.Length);
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public Blockette200(byte[] info, boolean swapBytes) throws SeedFormatException
		public Blockette200(sbyte[] info, bool swapBytes) : base(info, swapBytes)
		{
			trimToSize(Size);
		}

		public virtual string Name
		{
			get
			{
				return "Generic Event Detection Blockette";
			}
		}

		public virtual int Size
		{
			get
			{
				return B200_SIZE;
			}
		}

		public virtual int Type
		{
			get
			{
				return 200;
			}
		}

		/// <returns> - the signal amplitude field </returns>
		public virtual float Signal
		{
			get
			{
				return Float.intBitsToFloat(Utility.bytesToInt(info, SIGNAL, swapBytes));
			}
		}

		/// <returns> - the signal period field </returns>
		public virtual float Period
		{
			get
			{
				return Float.intBitsToFloat(Utility.bytesToInt(info, PERIOD, swapBytes));
			}
		}

		/// <returns> - the background estimate field </returns>
		public virtual float Background
		{
			get
			{
				return Float.intBitsToFloat(Utility.bytesToInt(info, BACKGROUND, swapBytes));
			}
		}

		/// <returns> - the signal onset time field </returns>
		public virtual Btime SignalOnset
		{
			get
			{
				return new Btime(info, SIGNAL_ONSET);
			}
		}

		public virtual string EventDetector
		{
			get
			{
				return new string(info, EVENT_DETECTOR, EVENT_DETECTOR_LENGTH);
			}
		}

		public virtual void writeASCII(PrintWriter @out)
		{
			@out.println("Blockette200 sig=" + Signal + " per=" + Period + " bkgrd=" + Background);
		}

		public override bool Equals(object o)
		{
			if (o == this)
			{
				return true;
			}
			if (o is Blockette200)
			{
				sbyte[] oinfo = ((Blockette200)o).info;
				if (info.length != oinfo.Length)
				{
					return false;
				}
				for (int i = 0; i < oinfo.Length; i++)
				{
					if (info[i] != oinfo[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Offsets for various fields
		private const int SIGNAL = 4;

		private const int PERIOD = 8;

		private const int BACKGROUND = 12;

		private const int SIGNAL_ONSET = 18;

		private const int EVENT_DETECTOR = 28;

		// Full size of blockette 200
		private const int B200_SIZE = 52;

		private const int EVENT_DETECTOR_LENGTH = 24;
	}
	internal static class StringHelper
	{
		//----------------------------------------------------------------------------------
		//	This method replaces the Java String.substring method when 'start' is a
		//	method call or calculated value to ensure that 'start' is obtained just once.
		//----------------------------------------------------------------------------------
		public static string SubstringSpecial(this string self, int start, int end)
		{
			return self.Substring(start, end - start);
		}

		//------------------------------------------------------------------------------------
		//	This method is used to replace calls to the 2-arg Java String.startsWith method.
		//------------------------------------------------------------------------------------
		public static bool StartsWith(this string self, string prefix, int toffset)
		{
			return self.IndexOf(prefix, toffset, StringComparison.Ordinal) == toffset;
		}

		//------------------------------------------------------------------------------
		//	This method is used to replace most calls to the Java String.split method.
		//------------------------------------------------------------------------------
		public static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
		{
			string[] splitArray = RegularExpressions.Regex.Split(self, regexDelimiter);

			if (trimTrailingEmptyStrings)
			{
				if (splitArray.Length > 1)
				{
					for (int i = splitArray.Length; i > 0; i--)
					{
						if (splitArray[i - 1].Length > 0)
						{
							if (i < splitArray.Length)
								Array.Resize(ref splitArray, i);

							break;
						}
					}
				}
			}

			return splitArray;
		}

		//-----------------------------------------------------------------------------
		//	These methods are used to replace calls to some Java String constructors.
		//-----------------------------------------------------------------------------
		public static string NewString(sbyte[] bytes)
		{
			return NewString(bytes, 0, bytes.Length);
		}
		public static string NewString(sbyte[] bytes, int index, int count)
		{
			return Encoding.UTF8.GetString((byte[])(object)bytes, index, count);
		}
		public static string NewString(sbyte[] bytes, string encoding)
		{
			return NewString(bytes, 0, bytes.Length, encoding);
		}
		public static string NewString(sbyte[] bytes, int index, int count, string encoding)
		{
			return NewString(bytes, index, count, Encoding.GetEncoding(encoding));
		}
		public static string NewString(sbyte[] bytes, Encoding encoding)
		{
			return NewString(bytes, 0, bytes.Length, encoding);
		}
		public static string NewString(sbyte[] bytes, int index, int count, Encoding encoding)
		{
			return encoding.GetString((byte[])(object)bytes, index, count);
		}

		//--------------------------------------------------------------------------------
		//	These methods are used to replace calls to the Java String.getBytes methods.
		//--------------------------------------------------------------------------------
		public static sbyte[] GetBytes(this string self)
		{
			return GetSBytesForEncoding(Encoding.UTF8, self);
		}
		public static sbyte[] GetBytes(this string self, Encoding encoding)
		{
			return GetSBytesForEncoding(encoding, self);
		}
		public static sbyte[] GetBytes(this string self, string encoding)
		{
			return GetSBytesForEncoding(Encoding.GetEncoding(encoding), self);
		}
		private static sbyte[] GetSBytesForEncoding(Encoding encoding, string s)
		{
			sbyte[] sbytes = new sbyte[encoding.GetByteCount(s)];
			encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
			return sbytes;
		}

	}
