﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
	public class Blockette2000 : DataBlockette
	{

		public Blockette2000(string[] headerFields, byte[] opaqueData) : base(opaqueData.Length + FIXED_HEADER_LENGTH + calcHeaderFieldLength(headerFields))
		{
			Array.Copy(Utility.intToByteArray(info.Length), 2, info, BLOCKETTE_LENGTH, 2);
			info[NUM_HEADER_FIELD] = (byte)headerFields.Length;
			int pos = HEADER_FIELD;
			for (int i = 0; i < headerFields.Length; i++)
			{
				byte[] headerBytes;
				try
				{
					headerBytes = (headerFields[i] + '~').GetBytes("US-ASCII");
				}
				catch (UnsupportedEncodingException)
				{
					throw new Exception("Java was unable to find the US-ASCII character encoding.");
				}
				Array.Copy(headerBytes, 0, info, pos, headerBytes.Length);
				pos += headerBytes.Length;
			}
			info[OPAQUE_OFFSET] = (byte)pos;
			Array.Copy(opaqueData, 0, info, pos, opaqueData.Length);
		}

		private static int calcHeaderFieldLength(string[] headerFields)
		{
			int len = headerFields.Length; // A byte for each terminating '~'
			for (int i = 0; i < headerFields.Length; i++)
			{
				len += headerFields[i].Length;
			}
			return len;
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public Blockette2000(byte[] info, boolean swapBytes) throws SeedFormatException
		public Blockette2000(byte[] info, bool swapBytes) : base(info, swapBytes)
		{
			checkMinimumSize(FIXED_HEADER_LENGTH);
			int size = Utility.uBytesToInt(info[4], info[5], swapBytes);
			trimToSize(size);
		}

		public override string Name
		{
			get
			{
				return "Variable Length Opaque Data Blockette";
			}
		}

		public override int Size
		{
			get
			{
				return info.Length;
			}
		}

		public override int Type
		{
			get
			{
				return 2000;
			}
		}

		public virtual string getHeaderField(int i)
		{
			int curHeader = 0;
			int start = HEADER_FIELD;
			for (; start < info.Length && curHeader != i; start++)
			{
				if (info[start] == '~')
				{
					curHeader++;
				}
			}
			int end = start;
			while (info[end] != '~')
			{
				end++;
			}
			return new string(info, start, end - start);
		}

		public virtual int NumHeaders
		{
			get
			{
				return info[NUM_HEADER_FIELD];
			}
		}

		public virtual byte[] OpaqueData
		{
			get
			{
				byte[] opaque = new byte[info.Length - info[OPAQUE_OFFSET]];
				Array.Copy(info, info[OPAQUE_OFFSET], opaque, 0, opaque.Length);
				return opaque;
			}
		}

		public override void WriteASCII(TextWriter @out)
		{
			@out.Write("Blockette2000 numHeaders=" + NumHeaders + " ");
			for (int i = 0; i < NumHeaders; i++)
			{
				@out.Write(getHeaderField(i) + ",");
			}
			@out.WriteLine(" " + OpaqueData.Length + " bytes of opaque (binary) data");
		}

		public override bool Equals(object o)
		{
			if (o == this)
			{
				return true;
			}
			if (o is Blockette2000)
			{
				byte[] oinfo = ((Blockette2000)o).info;
				if (info.Length != oinfo.Length)
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

		private const int BLOCKETTE_LENGTH = 4;

		private const int OPAQUE_OFFSET = 6;

		private const int NUM_HEADER_FIELD = 14;

		private const int HEADER_FIELD = 15;

		private const int FIXED_HEADER_LENGTH = 15;
	}

}
