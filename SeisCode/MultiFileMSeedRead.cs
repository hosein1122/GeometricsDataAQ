using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	/// <summary>
	/// @deprecated
	/// @author crotwell
	/// 
	/// </summary>
	[Obsolete]
	public class MultiFileMSeedRead : MiniSeedRead
	{

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public MultiFileMSeedRead(java.io.File[] files) throws java.io.IOException
		public MultiFileMSeedRead(File[] files)
		{
			this.files = files;
			initNextFile();
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void close() throws java.io.IOException
		public virtual void close()
		{
			if (current != null)
			{
				current.close();
			}
			currentIndex = files.Length; // make sure no read after close
		}

		/// <summary>
		/// gets the next logical record int the seed volume. This may not
		/// exactly correspond to the logical record structure within the
		/// volume as "continued" records will be concatinated to avoid
		/// partial blockettes. 
		/// </summary>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public SeedRecord getNextRecord() throws SeedFormatException, java.io.IOException
		public virtual SeedRecord NextRecord
		{
			get
			{
				if (current == null)
				{
					throw new EOFException("Cannot read past end of file list");
				}
				try
				{
					SeedRecord d = current.NextRecord;
					numReadTotal++;
					return d;
				}
				catch (EOFException)
				{
					// try next file
					initNextFile();
					SeedRecord d = current.NextRecord;
					numReadTotal++;
					return d;
				}
			}
		}

		public virtual int NumRecordsRead
		{
			get
			{
				return numReadTotal;
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: protected void initNextFile() throws java.io.IOException
		protected internal virtual void initNextFile()
		{
			if (currentIndex < files.Length)
			{
				if (current != null)
				{
					current.close();
					current = null;
				}
				current = new MiniSeedRead(new DataInputStream(new BufferedInputStream(new FileStream(files[currentIndex], FileMode.Open, FileAccess.Read))));
				currentIndex++;
			}
		}

		internal int numReadTotal = 0;

		internal int currentIndex = 0;

		internal File[] files;

		internal MiniSeedRead current;

		internal int current_record = 0;


	}

}
