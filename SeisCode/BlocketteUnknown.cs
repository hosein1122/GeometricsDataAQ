using System;
using System.Collections.Generic;
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

		public BlocketteUnknown(sbyte[] info, int type, bool swapBytes)
		{
			this.info = info;
			this.type = type;
			this.swapBytes = swapBytes;
		}

		public virtual int Type
		{
			get
			{
				return type;
			}
		}

		public virtual string Name
		{
			get
			{
				return "Unknown";
			}
		}

		public virtual int Size
		{
			get
			{
				return info.Length;
			}
		}

		public virtual int calcSize()
		{
			sbyte[] lengthBytes = new sbyte[4];
			Array.Copy(info, 3, lengthBytes, 0, 4);
			return int.Parse(StringHelper.NewString(lengthBytes));
		}

		public virtual sbyte[] toBytes()
		{
			return info;
		}

		public virtual void writeASCII(PrintWriter @out)
		{
			string infoStr = StringHelper.NewString(info);
			@out.println("Blockette " + Type + ": " + infoStr);
		}

		public virtual bool SwapBytes
		{
			get
			{
				return swapBytes;
			}
		}

		protected internal int type;

		protected internal sbyte[] info;

		protected internal bool swapBytes;
	} // BlocketteUnknown
}

//Helper class added by Java to C# Converter:

//-------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2020 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to convert some aspects of the Java String class.
//-------------------------------------------------------------------------------------------


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
