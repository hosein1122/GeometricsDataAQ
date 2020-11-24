using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class ControlHeader
	{

		protected internal int sequenceNum;

		protected internal sbyte typeCode;

		protected internal bool continuationCode;

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public static ControlHeader read(java.io.DataInput in) throws java.io.IOException, SeedFormatException
		public static ControlHeader read(DataInput @in)
		{
			sbyte[] seqBytes = new sbyte[6];
			@in.readFully(seqBytes);
			string seqNumString = StringHelper.NewString(seqBytes);

			int sequenceNum = 0;
			// check for blank string, leave as zero if so
			if (!seqNumString.Equals("      "))
			{
				try
				{
					sequenceNum = Convert.ToInt32(seqNumString);
				}
				catch (System.FormatException e)
				{
					Console.Error.WriteLine("seq num unreadable, setting to 0 " + e.ToString());
				} // end of try-catch
			}

			sbyte typeCode = @in.readByte();

			int b = @in.readByte();
			bool continuationCode;
			if (b == 32)
			{
				// a space, so no continuation
				continuationCode = false;
			}
			else if (b == 42)
			{
				// an asterisk, so is a continuation
				continuationCode = true;
			}
			else
			{
				throw new SeedFormatException("ControlHeader, expected space or *, but got " + b);
			}

			if (typeCode == (sbyte)'D' || typeCode == (sbyte)'R' || typeCode == (sbyte)'Q' || typeCode == (sbyte)'M')
			{
				// Data Header is D, R, Q or M
				return DataHeader.read(@in, sequenceNum, (char)typeCode, continuationCode);
			}
			else
			{
				// Control header is V, A, S, or T
				return new ControlHeader(sequenceNum, typeCode, continuationCode);
			}
		}

		/// <summary>
		/// This method writes Control Header into the output stream
		/// While writing, it will conform to the format of MiniSeed
		/// </summary>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: protected void write(java.io.DataOutput dos) throws java.io.IOException
		protected internal virtual void write(DataOutput dos)
		{
			DecimalFormat sequenceNumFormat = new DecimalFormat("000000");
			string sequenceNumString = sequenceNumFormat.format(SequenceNum);
			sbyte[] sequenceNumByteArray = null;
			try
			{
				sequenceNumByteArray = sequenceNumString.GetBytes(Encoding.ASCII);
			}
			catch (java.io.UnsupportedEncodingException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

			sbyte continuationCodeByte;
			if (continuationCode == true)
			{
				//if it is continuation,it is represented as asterix '*'
				continuationCodeByte = (sbyte)42;
			}
			else
			{
				//if it continuationCode is false...it is represented as space ' '
				continuationCodeByte = (sbyte)32;
			}
			try
			{
				dos.write(sequenceNumByteArray);
				dos.write((sbyte)typeCode);
				dos.write(continuationCodeByte);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

		}


		/// <summary>
		/// Writes an ASCII version of the record header. This is not meant to be a definitive ascii representation,
		/// merely to give something to print for debugging purposes. Ideally each field of the header should
		/// be printed in the order is appears in the header in a visually appealing way.
		/// </summary>
		/// <param name="out">
		///            a Writer
		///  </param>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void writeASCII(java.io.PrintWriter out) throws java.io.IOException
		public virtual void writeASCII(PrintWriter @out)
		{
			writeASCII(@out, "");
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void writeASCII(java.io.PrintWriter out, String indent) throws java.io.IOException
		public virtual void writeASCII(PrintWriter @out, string indent)
		{
			@out.print(indent + "seq=" + SequenceNum);
			@out.print(" type=" + TypeCode);
			@out.println(" cont=" + Continuation);
		}

		public ControlHeader(int sequenceNum, sbyte typeCode, bool continuationCode)
		{
			this.sequenceNum = sequenceNum;
			this.typeCode = (sbyte)typeCode;
			this.continuationCode = continuationCode;
		}

		public ControlHeader(int sequenceNum, char typeCode, bool continuationCode) : this(sequenceNum, (sbyte)typeCode, continuationCode)
		{
		}

		public virtual short Size
		{
			get
			{
				return 8;
			}
		}

		public virtual int SequenceNum
		{
			get
			{
				return sequenceNum;
			}
		}

		public virtual char TypeCode
		{
			get
			{
				return (char)typeCode;
			}
		}

		public virtual bool Continuation
		{
			get
			{
				return continuationCode;
			}
		}

		public override string ToString()
		{
			return TypeCode + "  " + SequenceNum;
		}
		public static void tester(string fileName)
		{

			DataOutputStream dos = null;
			try
			{
				dos = new DataOutputStream(new BufferedOutputStream(new FileStream(fileName, FileMode.Create, FileAccess.Write)));
				ControlHeader controlHeaderObject = new ControlHeader(23, (sbyte)'D', true);
				controlHeaderObject.write(dos);
				dos.close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
		}
		public static void Main(string[] args)
		{
			ControlHeader.tester(args[0]);

		}
	}

}

}
