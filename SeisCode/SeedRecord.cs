using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
    /// <summary>
    /// SeedRecord.java
    /// 
    /// 
    /// Created: Thu Apr 8 11:54:07 1999
    /// 
    /// @author Philip Crotwell
    /// @version
    /// </summary>

    public abstract class SeedRecord
    {

        public static BlocketteFactory BlocketteFactory
        {
            set
            {
                blocketteFactory = value;
            }
            get
            {
                return blocketteFactory;
            }
        }


        public static SeedRecord Read(BinaryReader inStream)
        {
            return Read(inStream, 0);
        }


        public static SeedRecord Read(byte[] bytes)
        {
            BinaryReader seedIn = new BinaryReader(new MemoryStream(bytes));
            return DataRecord.Read(seedIn);

            //DataInputStream seedIn = new DataInputStream(new MemoryStream(bytes));
            //return DataRecord.read(seedIn);

        }

        /// <summary>
        /// allows setting of a default record size, making reading of miniseed that
        /// lack a Blockette1000. Compression is still unknown, but at least the
        /// record can be read in and manipulated. A value of 0 for defaultRecordSize
        /// means there must be a blockette 1000 or a MissingBlockette1000 will be
        /// thrown.
        /// 
        /// If an exception is thrown and the underlying stream supports it, the stream
        /// will be reset to its state prior to any bytes being read. The buffer in the
        /// underlying stream must be large enough buffer any values read prior to the
        /// exception. A buffer sized to be the largest seed record expected is sufficient
        /// and so 4096 is a reasonable buffer size.
        /// </summary>

        public static SeedRecord Read(BinaryReader inStream, int defaultRecordSize)
        {
            bool resetOnError = true;// = inStream is BinaryReader && ((Stream)inStream).markSupported();
            if (resetOnError)
            {
                //((Stream)inStream).mark((defaultRecordSize != 0 ? defaultRecordSize : 4096)); // do twice in case of continuation record?
                //TODO Check this part.
                // byte[] buff =new byte[4096];
                // instream2.BaseStream.Read(buff,0,4096);
                // instream2 = new BinaryReader(new MemoryStream(buff));
            }
            try
            {
                ControlHeader header = ControlHeader.Read(inStream);
                SeedRecord newRecord;
                if (header is DataHeader)
                {
                    newRecord = DataRecord.ReadDataRecord(inStream, (DataHeader)header, defaultRecordSize);
                }
                else
                {
                    ControlRecord contRec = ControlRecord.ReadControlRecord(inStream, header, defaultRecordSize);
                    defaultRecordSize = contRec.RecordSize; // in case of b8 or b5 setting record size
                    newRecord = contRec;
                }
                return newRecord;
            }
            catch (SeedFormatException e)
            {
                if (resetOnError)
                {
                    try
                    {
                        //(inStream).reset();
                        _ = inStream.ReadByte();
                    }
                    catch (IOException)
                    {
                        throw e;
                    }
                }
                throw e;
            }
            catch (IOException e)
            {
                if (resetOnError)
                {
                    try
                    {
                        //((Stream)inStream).reset();
                    }
                    catch (IOException)
                    {
                        throw e;
                    }
                }
                throw e;
            }
            catch (Exception e)
            {
                if (resetOnError)
                {
                    try
                    {
                        // ((Stream)inStream).reset();
                    }
                    catch (IOException)
                    {
                        throw e;
                    }
                }
                throw e;
            }
        }

        public SeedRecord(ControlHeader header)
        {
            this.header = header;
        }


        public virtual void AddBlockette(Blockette b)
        {
            blockettes.Add(b);
        }

        public virtual Blockette[] Blockettes
        {
            get
            {
                return ((List<Blockette>)blockettes).ToArray();
            }
        }


        public virtual Blockette GetUniqueBlockette(int type)
        {
            Blockette[] b = GetBlockettes(type);
            if (b.Length == 1)
            {
                return b[0];
            }
            else if (b.Length == 0)
            {
                if (type == 1000)
                {
                    // special case as b1000 is required in mseed
                    throw new MissingBlockette1000(ControlHeader);
                }
                throw new SeedFormatException("No blockettes of type " + type);
            }
            else
            {
                throw new SeedFormatException("Multiple blockettes of type " + type);
            }
        }

        public virtual int GetNumBlockettes(int type)
        {
            int @out = 0;
            foreach (Blockette b in blockettes)
            {
                if (b.Type == type)
                {
                    @out++;
                }
            }
            return @out;
        }

        public virtual Blockette[] GetBlockettes(int type)
        {
            IList<Blockette> @out = new List<Blockette>();
            foreach (Blockette b in blockettes)
            {
                if (b.Type == type)
                {
                    @out.Add(b);
                }
            }
            return ((List<Blockette>)@out).ToArray();
        }

        /// <summary>
        /// if a seed blockette is continued in this record, a PartialBlockette will
        /// exist here. It will know its type and length, but will not have all its needed
        /// bytes. The prior Seed Record, possibly with reading the subsequent Seed Record should allow the remaining portion
        /// of the data to be read. This returns null in the case of no first partial blockette
        /// existing.
        /// </summary>
        public virtual PartialBlockette FirstPartialBlockette
        {
            get
            {
                if (blockettes[0] is PartialBlockette)
                {
                    return (PartialBlockette)blockettes[0];
                }
                return null;
            }
        }

        /// <summary>
        /// if a seed blockette is continued in the next record, a PartialBlockette will
        /// exist here. It will know its type and length, but will not have all its needed
        /// bytes. Reading the subsequent Seed Record should allow the remaining portion
        /// of the data to be read. This returns null in the case of no partial blockette
        /// existing.
        /// </summary>
        public virtual PartialBlockette LastPartialBlockette
        {
            get
            {
                if (blockettes.Count != 0 && blockettes[blockettes.Count - 1] is PartialBlockette)
                {
                    return (PartialBlockette)blockettes[blockettes.Count - 1];
                }
                return null;
            }
        }

        public virtual ControlHeader ControlHeader
        {
            get
            {
                return header;
            }
        }

        public override string ToString()
        {

            Stream st = new MemoryStream();
            TextWriter p = new StreamWriter(st);
            try
            {
                WriteASCII(p);
            }
            catch (IOException e)
            {
                // dont think this should happen
                throw new Exception(e.Message, e);
            }
            return st.ToString();
        }

        public virtual void WriteASCII(TextWriter @out)
        {
            WriteASCII(@out, "");
        }

        public virtual void WriteASCII(TextWriter @out, string indent)
        {
            if (this is DataRecord)
            {
                @out.Write(indent + "DataRecord");
            }
            else if (this is ControlRecord)
            {
                @out.Write(indent + "ControlRecord");
            }
            else
            {
                @out.Write(indent + "SeedRecord");
            }
            ControlHeader.WriteASCII(@out, indent + "  ");
            foreach (Blockette b in blockettes)
            {
                b.WriteASCII(@out, indent + "    ");
            }
        }

        public virtual int RecordSize
        {
            get
            {
                return RECORD_SIZE;
            }
            set { }

        }

        protected internal ControlHeader header;

        protected internal IList<Blockette> blockettes = new List<Blockette>();

        protected internal static BlocketteFactory blocketteFactory = new DefaultBlocketteFactory();

        protected internal int RECORD_SIZE = 4096;
    } // SeedRecord

}
