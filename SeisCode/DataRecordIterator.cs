using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
    public class DataRecordIterator
    {

        public DataRecordIterator(BinaryReader @in)
        {
            this.@in = @in;
        }

        public virtual bool HasNext()
        {
            if (@in == null)
            {
                return false;
            }
            while (@in != null && nextDr == null)
            {
                try
                {
                    SeedRecord sr = SeedRecord.Read(@in);
                    if (sr is DataRecord)
                    {
                        nextDr = (DataRecord)sr;
                        break;
                    }
                    else
                    {
                        Logger.Warning("Not a data record, skipping..." + sr.ControlHeader.SequenceNum + " " + sr.ControlHeader.TypeCode);
                    }
                }
                catch (IOException)
                {
                    @in = null;
                    nextDr = null;
                    break;
                }
            }
            return nextDr != null;
        }

        public virtual DataRecord Next()
        {
            if (HasNext())
            {
                DataRecord @out = nextDr;
                nextDr = null;
                return @out;
            }
            return null;
        }

        public virtual void Close()
        {
            if (@in != null && @in is BinaryReader)
            {
                try
                {
                    @in.Close();
                }
                catch (IOException)
                {
                    // oh well...
                }
                @in = null;
            }
        }

        ~DataRecordIterator()
        {
            Close();
        }

        internal DataRecord nextDr;

        internal BinaryReader @in;

    }

}
