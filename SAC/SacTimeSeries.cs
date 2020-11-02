using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAC
{
    public class SacTimeSeries
    {
        public SacTimeSeries()
        {
        }

        /// <summary>
        /// create a new SAC timeseries from the given header and data. The header values
        /// related to the data are set correctly:
        /// <ul>
        ///  <li>npts=data.length</li>
        ///  <li>e=b+(npts-1)*delta</li>
        ///  <li>iftype=ITIME</li>
        ///  <li>leven=TRUE</li>
        /// </ul>
        ///  Setting of all other headers is the responsibility of the caller. </summary>
        /// <param name="header"> </param>
        /// <param name="data"> </param>
        //JAVA TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
        //ORIGINAL LINE: public SacTimeSeries(SacHeader header, float[] data)
        public SacTimeSeries(SacHeader header, float[] data)
        {
            this.header = header;
            header.Iftype = SacConstants.ITIME;
            header.Leven = SacConstants.TRUE;
            Y = data;
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public SacTimeSeries(File file) throws FileNotFoundException, IOException
        //JAVA TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
        public SacTimeSeries(FileInfo file)
        {
            read(file);
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public SacTimeSeries(String filename) throws FileNotFoundException, IOException
        //JAVA TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
        public SacTimeSeries(string filename)
        {
            FileInfo file = new FileInfo(filename);
            read(file);
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public SacTimeSeries(DataInput inStream) throws IOException
        //JAVA TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
        public SacTimeSeries(BinaryReader inStream)
        {
            read(inStream);
        }

        public virtual float[] Y
        {
            get
            {
                return y;
            }
            set
            {
                this.y = value;
                Header.Npts = value.Length;
                if (!SacConstants.isUndef(Header.Delta) && !SacConstants.isUndef(Header.B))
                {
                    Header.E = Header.B + (value.Length - 1) * Header.Delta;
                }
            }
        }


        public virtual float[] X
        {
            get
            {
                return x;
            }
            set
            {
                this.x = value;
            }
        }


        public virtual float[] Real
        {
            get
            {
                return real;
            }
            set
            {
                this.real = value;
            }
        }


        public virtual float[] Imaginary
        {
            get
            {
                return imaginary;
            }
            set
            {
                this.imaginary = value;
            }
        }


        public virtual float[] Amp
        {
            get
            {
                return amp;
            }
            set
            {
                this.amp = value;
            }
        }


        public virtual float[] Phase
        {
            get
            {
                return phase;
            }
            set
            {
                this.phase = value;
            }
        }


        public virtual SacHeader Header
        {
            get
            {
                return header;
            }
        }

        public virtual void printHeader(TextWriter @out)
        {
            header.printHeader(@out);
        }

        public virtual int NumPtsRead
        {
            get
            {
                return numPtsRead;
            }
        }

        private SacHeader header;

        private float[] y;

        private float[] x;

        private float[] real;

        private float[] imaginary;

        private float[] amp;

        private float[] phase;

        private int numPtsRead = 0;





        /// <summary>
        /// reads the sac file specified by the filename. Only a very simple check is
        /// made to be sure the file really is a sac file.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        ///             if the file cannot be found </exception>
        /// <exception cref="IOException">
        ///             if it isn't a sac file or if it happens :) </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void read(String filename) throws FileNotFoundException, IOException
        public virtual void read(string filename)
        {
            FileInfo sacFile = new FileInfo(filename);
            read(sacFile);
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void read(File sacFile) throws FileNotFoundException, IOException
        public virtual void read(FileInfo sacFile)
        {
            if (sacFile.Length < SacConstants.data_offset)
            {
                throw new IOException(sacFile.Name + " does not appear to be a sac file! File size (" + sacFile.Length + " is less than sac's header size (" + SacConstants.data_offset + ")");
            }
            //DataInputStream dis = new DataInputStream(new BufferedInputStream(new FileStream(sacFile, FileMode.Open, FileAccess.Read)));
            BinaryReader dis = new BinaryReader(sacFile.OpenRead());
            try
            {
                header = new SacHeader(dis);
                if (header.Leven == 1 && header.Iftype == SacConstants.ITIME)
                {
                    if (sacFile.Length != header.Npts * 4 + SacConstants.data_offset)
                    {
                        throw new IOException(sacFile.Name + " does not appear to be a sac file! npts(" + header.Npts + ") * 4 + header(" + SacConstants.data_offset + ") !=  file length=" + sacFile.Length + "\n  as linux: npts(" + SacHeader.swapBytes(header.Npts) + ")*4 + header(" + SacConstants.data_offset + ") !=  file length=" + sacFile.Length);
                    }
                }
                else if (header.Leven == 1 || (header.Iftype == SacConstants.IAMPH || header.Iftype == SacConstants.IRLIM))
                {
                    if (sacFile.Length != header.Npts * 4 * 2 + SacConstants.data_offset)
                    {
                        throw new IOException(sacFile.Name + " does not appear to be a amph or rlim sac file! npts(" + header.Npts + ") * 4 *2 + header(" + SacConstants.data_offset + ") !=  file length=" + sacFile.Length + "\n  as linux: npts(" + SacHeader.swapBytes(header.Npts) + ")*4*2 + header(" + SacConstants.data_offset + ") !=  file length=" + sacFile.Length);
                    }
                }
                else if (header.Leven == 0 && sacFile.Length != header.Npts * 4 * 2 + SacConstants.data_offset)
                {
                    throw new IOException(sacFile.Name + " does not appear to be a uneven sac file! npts(" + header.Npts + ") * 4 *2 + header(" + SacConstants.data_offset + ") !=  file length=" + sacFile.Length + "\n  as linux: npts(" + SacHeader.swapBytes(header.Npts) + ")*4*2 + header(" + SacConstants.data_offset + ") !=  file length=" + sacFile.Length);
                }
                readData(dis);
            }
            finally
            {
                dis.Close();
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void read(DataInput dis) throws IOException
        public virtual void read(BinaryReader dis)
        {
            header = new SacHeader(dis);
            readData(dis);
        }

        /// <summary>
        /// read the data portion of the given File </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: protected void readData(DataInput fis) throws IOException
        protected internal virtual void readData(BinaryReader fis)
        {
            y = new float[header.Npts];
            readDataArray(fis, y, header.ByteOrder);
            if (header.Leven == SacConstants.FALSE || header.Iftype == SacConstants.IRLIM || header.Iftype == SacConstants.IAMPH)
            {
                x = new float[header.Npts];
                readDataArray(fis, x, header.ByteOrder);
                if (header.Iftype == SacConstants.IRLIM)
                {
                    real = y;
                    imaginary = x;
                }
                if (header.Iftype == SacConstants.IAMPH)
                {
                    amp = y;
                    phase = x;
                }
            }
            numPtsRead = header.Npts;
        }


        /// <summary>
        /// reads data.length floats. It is up to the caller to insure that the type
        /// of SAC file (iftype = LEVEN, IRLIM, IAMPH) and how many data points
        /// remain are compatible with the size of the float array to be read.
        /// </summary>
        /// <exception cref="IOException"> </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public static void readSomeData(DataInput dataIn, float[] data, boolean byteOrder) throws IOException
        public static void readSomeData(BinaryReader dataIn, float[] data, bool byteOrder)
        {
            readDataArray(dataIn, data, byteOrder);
        }

        /// <summary>
        /// skips samplesToSkip data points. It is up to the caller to insure that
        /// the type of SAC file (iftype = LEVEN, IRLIM, IAMPH) and how many data
        /// points remain are compatible with the size of the float array to be read.
        /// </summary>
        /// <exception cref="IOException"> </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public static int skipSamples(DataInput dataIn, int samplesToSkip) throws IOException
        public static int SkipSamples(BinaryReader dataIn, int samplesToSkip)
        {
            throw new Exception("not impelemented yet!!!");
            //return dataIn.skipBytes(samplesToSkip * 4) / 4;
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: private static void readDataArray(DataInput fis, float[] d, boolean byteOrder) throws IOException
        private static void readDataArray(BinaryReader fis, float[] d, bool byteOrder)
        {
            byte[] dataBytes = new byte[d.Length * 4];
            int numAdded = 0;
            int i = 0;
            dataBytes = fis.ReadBytes(dataBytes.Length);
            while (numAdded < d.Length)
            {
                if (byteOrder == SacConstants.IntelByteOrder)
                {
                    d[numAdded++] = intBitsToFloat(((dataBytes[i++] & 0xff) << 0) + ((dataBytes[i++] & 0xff) << 8) + ((dataBytes[i++] & 0xff) << 16) + ((dataBytes[i++] & 0xff) << 24));
                }
                else
                {
                    d[numAdded++] = intBitsToFloat(((dataBytes[i++] & 0xff) << 24) + ((dataBytes[i++] & 0xff) << 16) + ((dataBytes[i++] & 0xff) << 8) + ((dataBytes[i++] & 0xff) << 0));
                }
            }
        }

        /// <summary>
        /// writes this object out as a sac file. </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void write(String filename) throws FileNotFoundException, IOException
        public virtual void write(string filename)
        {
            FileInfo f = new FileInfo(filename);
            write(f);
        }

        /// <summary>
        /// writes this object out as a sac file. </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void write(File file) throws FileNotFoundException, IOException
        public virtual void write(FileInfo file)
        {
            //DataOutputStream dos = new DataOutputStream(new BufferedOutputStream(new FileStream(file, FileMode.Create, FileAccess.Write)));
            BinaryWriter dos = new BinaryWriter(file.OpenWrite());
            try
            {
                header.writeHeader(dos);
                writeData(dos);
            }
            finally
            {
                dos.Close();
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void writeData(DataOutput dos) throws IOException
        public virtual void writeData(BinaryWriter dos)
        {
            for (int i = 0; i < header.Npts; i++)
            {
                header.writeFloat(dos, y[i]);
            }
            if (header.Leven == SacConstants.FALSE || header.Iftype == SacConstants.IRLIM || header.Iftype == SacConstants.IAMPH)
            {
                for (int i = 0; i < header.Npts; i++)
                {
                    header.writeFloat(dos, x[i]);
                }
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public static void appendData(File outfile, float[] data) throws IOException
        public static void appendData(FileInfo outFile, float[] data)
        {
            //RandomAccessFile raFile = new RandomAccessFile(outfile, "rw");
            BinaryWriter raFile = new BinaryWriter(outFile.OpenWrite());
            try
            {
                SacHeader header = new SacHeader(outFile);
                if (header.Leven == SacConstants.FALSE || header.Iftype == SacConstants.IRLIM || header.Iftype == SacConstants.IAMPH)
                {
                    raFile.Close();
                    throw new IOException("Can only append to evenly sampled sac files, ie only Y");
                }
                int origNpts = header.Npts;
                header.Npts = header.Npts + data.Length;
                header.E = (header.Npts - 1) * header.Delta;
                raFile.Seek(0, 0);
                header.writeHeader(raFile);
                raFile.Seek(origNpts * 4, 0); // four bytes per float
                if (header.ByteOrder == SacConstants.LITTLE_ENDIAN)
                {
                    // Phil Crotwell's solution:
                    // careful here as dos.writeFloat() will collapse all NaN floats
                    // to
                    // a single NaN value. But we are trying to write out byte
                    // swapped values
                    // so different floats that are all NaN are different values in
                    // the
                    // other byte order. Solution is to swap on the integer bits,
                    // not the float.
                    for (int i = 0; i < data.Length; i++)
                    {
                        raFile.Write(SacHeader.swapBytes(floatToRawIntBits(data[i])));
                    }
                }
                else
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        raFile.Write(data[i]);
                    }
                }
            }
            finally
            {
                raFile.Close();
            }
        }

        /// <summary>
        /// just for testing. Reads the filename given as the argument, writes out
        /// some header variables and then writes it back out as "outsacfile".
        /// </summary>
        public static void Main(string[] args)
        {
            SacTimeSeries data = new SacTimeSeries();
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: java SacTimeSeries sacsourcefile ");
                return;
            }
            try
            {
                data.read(args[0]);
                // data.y = new float[100000];
                // for (int i=0; i<100000; i++) {
                // data.y[i] = (float)Math.sin(Math.PI*i/18000)/1000000.0f;
                // data.y[i] = (float)Math.sin(Math.PI*i/18000);
                // //System.out.println("point is " + data.y[i]);
                // }
                // data.npts = data.y.length;
                // data.printHeader();
                Console.WriteLine("stla original: " + data.header.Stla + " npts=" + data.header.Npts);
                // data.setLittleEndian();
                data.write("outsacfile");
                data.read("outsacfile");
                Console.WriteLine("stla after read little endian: " + data.header.Stla + " npts=" + data.header.Npts);
                Console.WriteLine("Done writing");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File " + args[0] + " doesn't exist.");
            }
            catch (IOException e)
            {
                Console.WriteLine("IOException: " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static int floatToRawIntBits(float value)
        {
            var buffer = new byte[4];
            BinaryWriter bw = new BinaryWriter(new MemoryStream(buffer));
            bw.Write(value);
            BinaryReader br = new BinaryReader(new MemoryStream(buffer));
            return br.ReadInt32();

        }

        private static float intBitsToFloat(int x)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            float f = BitConverter.ToSingle(bytes, 0);
            return f;
        }

    }
}
