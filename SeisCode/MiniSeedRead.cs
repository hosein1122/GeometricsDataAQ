using System;
using System.Collections.Generic;
using System.IO;
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

        public MiniSeedRead(BinaryReader inStream)
        {
            this.inStream = inStream;
        }

        public virtual void Close()
        {
            inStream = null;
        }

        /// <summary>
        /// gets the next logical record int the seed volume. This may not exactly
        /// correspond to the logical record structure within the volume as
        /// "continued" records will be concatinated to avoid partial blockettes.
        /// </summary>
        public virtual SeedRecord NextRecord
        {
            get
            {
                return GetNextRecord(0);
            }
        }

        /// <summary>
        /// gets the next logical record int the seed volume. This may not exactly
        /// correspond to the logical record structure within the volume as
        /// "continued" records will be concatinated to avoid partial blockettes.
        /// </summary>
        public virtual SeedRecord GetNextRecord(int defaultRecordSize)
        {
            return SeedRecord.Read(inStream, defaultRecordSize);
        }

        public virtual int NumRecordsRead
        {
            get
            {
                return numRead;
            }
        }

        protected internal int numRead = 0;

        protected internal BinaryReader inStream;

        protected internal int recordSize;

        protected internal bool readData;
        public static void ReadMiniSeed(string filename)
        {

            TextWriter @out = new StreamWriter("log.txt");
            int maxPackets = -1;
            try
            {
                @out.WriteLine("open socket");
                var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader ls = new BinaryReader(fs);
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
                        @out.WriteLine("Missing Blockette1000, trying with record size of 4096");
                        // try with 4096 as default
                        sr = rf.GetNextRecord(4096);
                    }
                    sr.WriteASCII(@out, "    ");
                    if (sr is DataRecord)
                    {
                        DataRecord dr = (DataRecord)sr;
                        byte[] data = dr.Data;
                        // should use seedCodec to do something with the data...
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("EOF, so done.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

        }
    } // MiniSeedRead

}
