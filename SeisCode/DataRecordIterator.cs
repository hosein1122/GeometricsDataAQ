using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	public class DataRecordIterator
	{

		public DataRecordIterator(DataInput @in)
		{
			this.@in = @in;
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public boolean hasNext() throws SeedFormatException, java.io.IOException
		public virtual bool hasNext()
		{
			if (@in == null)
			{
				return false;
			}
			while (@in != null && nextDr == null)
			{
				try
				{
					SeedRecord sr = SeedRecord.read(@in);
					if (sr is DataRecord)
					{
						nextDr = (DataRecord)sr;
						break;
					}
					else
					{
						logger.warn("Not a data record, skipping..." + sr.ControlHeader.SequenceNum + " " + sr.ControlHeader.TypeCode);
					}
				}
				catch (EOFException)
				{
					@in = null;
					nextDr = null;
					break;
				}
			}
			return nextDr != null;
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public DataRecord next() throws SeedFormatException, java.io.IOException
		public virtual DataRecord next()
		{
			if (hasNext())
			{
				DataRecord @out = nextDr;
				nextDr = null;
				return @out;
			}
			return null;
		}

		public virtual void close()
		{
			if (@in != null && @in is DataInputStream)
			{
				try
				{
					((DataInputStream)@in).close();
				}
				catch (IOException)
				{
					// oh well...
				}
				@in = null;
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: @Override protected void finalize() throws Throwable
		~DataRecordIterator()
		{
			//JAVA TO C# CONVERTER NOTE: The base class finalizer method is automatically called in C#:
			//			base.finalize();
			close();
		}

		internal DataRecord nextDr;

		internal DataInput @in;

		private static readonly org.slf4j.Logger logger = org.slf4j.LoggerFactory.getLogger(typeof(DataRecordIterator));

		/* this is to prevent the FdsnDataSelectQuerier from being garbage collected and closing the input stream. */
		internal FDSNDataSelectQuerier querier;
		public virtual FDSNDataSelectQuerier Querier
		{
			set
			{
				this.querier = value;
			}
		}
	}

}
