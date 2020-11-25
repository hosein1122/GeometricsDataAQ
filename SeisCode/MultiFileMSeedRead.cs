using System;
using System.Collections.Generic;
using System.IO;
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
        public MultiFileMSeedRead(FileInfo[] files)
        {
            this.files = files;
            InitNextFile();
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void close() throws java.io.IOException
        public override void close()
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
        public override SeedRecord NextRecord
        {
            get
            {
                if (current == null)
                {
                    throw new IOException("Cannot read past end of file list");
                }
                try
                {
                    SeedRecord d = current.NextRecord;
                    numReadTotal++;
                    return d;
                }
                catch (IOException)
                {
                    // try next file
                    InitNextFile();
                    SeedRecord d = current.NextRecord;
                    numReadTotal++;
                    return d;
                }
            }
        }

        public override int NumRecordsRead
        {
            get
            {
                return numReadTotal;
            }
        }

        protected internal virtual void InitNextFile()
        {
            if (currentIndex < files.Length)
            {
                if (current != null)
                {
                    current.close();
                    current = null;
                }
                current = new MiniSeedRead(new BinaryReader(new FileStream(files[currentIndex].FullName, FileMode.Open, FileAccess.Read)));
                currentIndex++;
            }
        }

        internal int numReadTotal = 0;

        internal int currentIndex = 0;

        internal FileInfo[] files;

        internal MiniSeedRead current;

        internal int current_record = 0;


    }

}
