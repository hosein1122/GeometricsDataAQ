using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	/// <summary>
	/// MiniSeedRead.java
	/// 
	/// 
	/// Created: Thu Apr 8 12:10:52 1999
	/// 
	/// @author Philip Crotwell
	/// @version
	/// </summary>

	/// @deprecated See ListHeader for an example client and SeedRecord.read for reading
	/// 
	/// @author crotwell
	///  
	[Obsolete("See ListHeader for an example client and SeedRecord.read for reading")]
	public class MiniSeedRead
	{

		protected internal MiniSeedRead()
		{
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public MiniSeedRead(java.io.DataInput inStream) throws java.io.IOException
		public MiniSeedRead(DataInput inStream)
		{
			this.inStream = inStream;
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void close() throws java.io.IOException
		public virtual void close()
		{
			inStream = null;
		}

		/// <summary>
		/// gets the next logical record int the seed volume. This may not exactly
		/// correspond to the logical record structure within the volume as
		/// "continued" records will be concatinated to avoid partial blockettes.
		/// </summary>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public SeedRecord getNextRecord() throws SeedFormatException, java.io.IOException
		public virtual SeedRecord NextRecord
		{
			get
			{
				return getNextRecord(0);
			}
		}
		/// <summary>
		/// gets the next logical record int the seed volume. This may not exactly
		/// correspond to the logical record structure within the volume as
		/// "continued" records will be concatinated to avoid partial blockettes.
		/// </summary>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public SeedRecord getNextRecord(int defaultRecordSize) throws SeedFormatException, java.io.IOException
		public virtual SeedRecord getNextRecord(int defaultRecordSize)
		{
			return SeedRecord.read(inStream, defaultRecordSize);
		}

		public virtual int NumRecordsRead
		{
			get
			{
				return numRead;
			}
		}

		protected internal int numRead = 0;

		protected internal DataInput inStream;

		protected internal int recordSize;

		protected internal bool readData;

		public static void Main(string[] args)
		{
			DataInputStream ls = null;
			PrintWriter @out = new PrintWriter(System.out, true);
			int maxPackets = -1;
			try
			{
				@out.println("open socket");
				if (args.Length == 0)
				{
					//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
					@out.println("Usage: java " + typeof(MiniSeedRead).FullName + " filename");
				}
				else
				{
					ls = new DataInputStream(new BufferedInputStream(new FileStream(args[0], FileMode.Open, FileAccess.Read), 4096));
				}
				MiniSeedRead rf = new MiniSeedRead(ls);
				for (int i = 0; maxPackets == -1 || i < maxPackets; i++)
				{
					SeedRecord sr;
					try
					{
						sr = rf.NextRecord;
					}
					catch (MissingBlockette1000)
					{
						@out.println("Missing Blockette1000, trying with record size of 4096");
						// try with 4096 as default
						sr = rf.getNextRecord(4096);
					}
					sr.writeASCII(@out, "    ");
					if (sr is DataRecord)
					{
						DataRecord dr = (DataRecord)sr;
						sbyte[] data = dr.Data;
						// should use seedCodec to do something with the data...
					}
				}
			}
			catch (EOFException)
			{
				Console.WriteLine("EOF, so done.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			finally
			{
				try
				{
					if (ls != null)
					{
						ls.close();
					}
				}
				catch (Exception)
				{
				}
			}
		}
	} // MiniSeedRead

}
