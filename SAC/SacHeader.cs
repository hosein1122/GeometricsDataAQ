using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAC
{
    public class SacHeader
    {
        public SacHeader()
        {
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public SacHeader(String filename) throws IOException
        public SacHeader(string filename)
        {
            BinaryReader sacFile = new BinaryReader(new FileStream(filename, FileMode.Open));
            readHeader(sacFile);
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public SacHeader(DataInput indis) throws IOException
        public SacHeader(BinaryReader indis)
        {
            readHeader(indis);
        }

        /// <summary>
        /// reads just the sac header specified by the filename. Limited checks are made
        /// to be sure the file really is a sac file.
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public SacHeader(File sacFile) throws IOException
        public SacHeader(FileInfo sacFile)
        {

            if (sacFile.Length < SacConstants.data_offset)
            {
                throw new System.IO.IOException(sacFile.Name + " does not appear to be a sac file! File size (" + sacFile.Length + " is less than sac's header size (" + SacConstants.data_offset + ")");
            }
            //DataInputStream dis = new DataInputStream(new BufferedInputStream(new FileInputStream(sacFile)));
            BinaryReader dis = new BinaryReader(sacFile.OpenRead());
            try
            {
                readHeader(dis);
            }
            finally
            {
                dis.Close();
            }
        }

        public static SacHeader createEmptyEvenSampledTimeSeriesHeader()
        {
            SacHeader header = new SacHeader();
            header.leven = SacConstants.TRUE;
            header.iftype = SacConstants.ITIME;
            header.npts = 0;
            header.b = 0.0f;
            header.e = 0.0f;
            header.idep = SacConstants.IUNKN;
            return header;
        }

        /// <summary>
        /// reads the header from the given stream. The NVHDR value (should be 6) is
        /// checked to see if byte swapping is needed. If so, all header values are
        /// byte swapped and the byteOrder is set to IntelByteOrder (false) so that
        /// the data section will also be byte swapped on read. Extra care is taken
        /// to do all byte swapping before the byte values are transformed into
        /// floats as java can do very funny things if the byte-swapped float happens
        /// to be a NaN.
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: void readHeader(DataInput indis) throws IOException
        internal virtual void readHeader(BinaryReader indis)
        {
            byte[] headerBuf = new byte[SacConstants.data_offset];
            headerBuf = indis.ReadBytes(headerBuf.Length);
            if (headerBuf[SacConstants.NVHDR_OFFSET] == 0 && headerBuf[SacConstants.NVHDR_OFFSET + 1] == 0 && headerBuf[SacConstants.NVHDR_OFFSET + 2] == 0 && headerBuf[SacConstants.NVHDR_OFFSET + 3] == 6)
            {
                byteOrder = SacConstants.IntelByteOrder;
                // little endian byte order, swap bytes on first 110 4-byte values
                // in header, rest are text
                for (int i = 0; i < 110 * 4; i += 4)
                {
                    byte tmp = headerBuf[i];
                    headerBuf[i] = headerBuf[i + 3];
                    headerBuf[i + 3] = tmp;
                    tmp = headerBuf[i + 1];
                    headerBuf[i + 1] = headerBuf[i + 2];
                    headerBuf[i + 2] = tmp;
                }
            }
            else if (!(headerBuf[SacConstants.NVHDR_OFFSET] == 6 && headerBuf[SacConstants.NVHDR_OFFSET + 1] == 0 && headerBuf[SacConstants.NVHDR_OFFSET + 2] == 0 && headerBuf[SacConstants.NVHDR_OFFSET + 3] == 0))
            {
                throw new System.IO.IOException("Does not appear to be a SAC file, NVHDR header bytes should be (int) 6 but found " + headerBuf[SacConstants.NVHDR_OFFSET] + " " + headerBuf[SacConstants.NVHDR_OFFSET + 1] + " " + headerBuf[SacConstants.NVHDR_OFFSET + 2] + " " + headerBuf[SacConstants.NVHDR_OFFSET + 3]);
            }
            // DataInputStream dis = new DataInputStream(new ByteArrayInputStream(headerBuf));
            BinaryReader dis = new BinaryReader(new MemoryStream(headerBuf));
            delta = dis.ReadSingle();
            depmin = dis.ReadSingle();
            depmax = dis.ReadSingle();
            scale = dis.ReadSingle();
            odelta = dis.ReadSingle();
            b = dis.ReadSingle();
            e = dis.ReadSingle();
            o = dis.ReadSingle();
            a = dis.ReadSingle();
            fmt = dis.ReadSingle();
            t0 = dis.ReadSingle();
            t1 = dis.ReadSingle();
            t2 = dis.ReadSingle();
            t3 = dis.ReadSingle();
            t4 = dis.ReadSingle();
            t5 = dis.ReadSingle();
            t6 = dis.ReadSingle();
            t7 = dis.ReadSingle();
            t8 = dis.ReadSingle();
            t9 = dis.ReadSingle();
            f = dis.ReadSingle();
            resp0 = dis.ReadSingle();
            resp1 = dis.ReadSingle();
            resp2 = dis.ReadSingle();
            resp3 = dis.ReadSingle();
            resp4 = dis.ReadSingle();
            resp5 = dis.ReadSingle();
            resp6 = dis.ReadSingle();
            resp7 = dis.ReadSingle();
            resp8 = dis.ReadSingle();
            resp9 = dis.ReadSingle();
            stla = dis.ReadSingle();
            stlo = dis.ReadSingle();
            stel = dis.ReadSingle();
            stdp = dis.ReadSingle();
            evla = dis.ReadSingle();
            evlo = dis.ReadSingle();
            evel = dis.ReadSingle();
            evdp = dis.ReadSingle();
            mag = dis.ReadSingle();
            user0 = dis.ReadSingle();
            user1 = dis.ReadSingle();
            user2 = dis.ReadSingle();
            user3 = dis.ReadSingle();
            user4 = dis.ReadSingle();
            user5 = dis.ReadSingle();
            user6 = dis.ReadSingle();
            user7 = dis.ReadSingle();
            user8 = dis.ReadSingle();
            user9 = dis.ReadSingle();
            dist = dis.ReadSingle();
            az = dis.ReadSingle();
            baz = dis.ReadSingle();
            gcarc = dis.ReadSingle();
            sb = dis.ReadSingle();
            sdelta = dis.ReadSingle();
            depmen = dis.ReadSingle();
            cmpaz = dis.ReadSingle();
            cmpinc = dis.ReadSingle();
            xminimum = dis.ReadSingle();
            xmaximum = dis.ReadSingle();
            yminimum = dis.ReadSingle();
            ymaximum = dis.ReadSingle();
            unused6 = dis.ReadSingle();
            unused7 = dis.ReadSingle();
            unused8 = dis.ReadSingle();
            unused9 = dis.ReadSingle();
            unused10 = dis.ReadSingle();
            unused11 = dis.ReadSingle();
            unused12 = dis.ReadSingle();
            nzyear = dis.ReadInt32();
            nzjday = dis.ReadInt32();
            nzhour = dis.ReadInt32();
            nzmin = dis.ReadInt32();
            nzsec = dis.ReadInt32();
            nzmsec = dis.ReadInt32();
            nvhdr = dis.ReadInt32();
            norid = dis.ReadInt32();
            nevid = dis.ReadInt32();
            npts = dis.ReadInt32();
            nsnpts = dis.ReadInt32();
            nwfid = dis.ReadInt32();
            nxsize = dis.ReadInt32();
            nysize = dis.ReadInt32();
            unused15 = dis.ReadInt32();
            iftype = dis.ReadInt32();
            idep = dis.ReadInt32();
            iztype = dis.ReadInt32();
            unused16 = dis.ReadInt32();
            iinst = dis.ReadInt32();
            istreg = dis.ReadInt32();
            ievreg = dis.ReadInt32();
            ievtyp = dis.ReadInt32();
            iqual = dis.ReadInt32();
            isynth = dis.ReadInt32();
            imagtyp = dis.ReadInt32();
            imagsrc = dis.ReadInt32();
            unused19 = dis.ReadInt32();
            unused20 = dis.ReadInt32();
            unused21 = dis.ReadInt32();
            unused22 = dis.ReadInt32();
            unused23 = dis.ReadInt32();
            unused24 = dis.ReadInt32();
            unused25 = dis.ReadInt32();
            unused26 = dis.ReadInt32();
            leven = dis.ReadInt32();
            lpspol = dis.ReadInt32();
            lovrok = dis.ReadInt32();
            lcalda = dis.ReadInt32();
            unused27 = dis.ReadInt32();
            byte[] eightBytes = new byte[8];
            byte[] sixteenBytes = new byte[16];
            eightBytes = dis.ReadBytes(8);
            kstnm = BitConverter.ToString(eightBytes);
            sixteenBytes = dis.ReadBytes(16);
            kevnm = BitConverter.ToString(sixteenBytes);
            eightBytes = dis.ReadBytes(8);
            khole = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            ko = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            ka = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt0 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt1 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt2 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt3 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt4 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt5 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt6 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt7 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt8 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kt9 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kf = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kuser0 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kuser1 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kuser2 = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kcmpnm = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            knetwk = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kdatrd = BitConverter.ToString(eightBytes);
            eightBytes = dis.ReadBytes(8);
            kinst = BitConverter.ToString(eightBytes);
        }


        /// <summary>
        /// write the float to the stream, swapping bytes if needed. </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: final void writeFloat(DataOutput dos, float val) throws IOException
        internal void writeFloat(/*DataOutput*/ BinaryWriter dos, float val)
        {
            if (byteOrder == SacConstants.IntelByteOrder)
            {
                // careful here as dos.writeFloat() will collapse all NaN floats to
                // a single NaN value. But we are trying to write out byte swapped
                // values
                // so different floats that are all NaN are different values in the
                // other byte order. Solution is to swap on the integer bits, not
                // the float
                dos.Write(swapBytes(floatToRawIntBits(val)));

            }
            else
            {
                dos.Write(val);
            } // end of else
        }

        /// <summary>
        /// write the float to the stream, swapping bytes if needed. </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: private final void writeInt(DataOutput dos, int val) throws IOException
        private void writeInt(/*DataOutput*/ BinaryWriter dos, int val)
        {
            if (byteOrder == SacConstants.IntelByteOrder)
            {
                dos.Write(swapBytes(val));
            }
            else
            {
                dos.Write(val);
            } // end of else
        }

        /// <summary>
        /// writes this object out as a sac file. </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void writeHeader(File file) throws FileNotFoundException, IOException
        public virtual void writeHeader(FileInfo file)
        {
            //DataOutputStream dos = new DataOutputStream(new BufferedOutputStream(new FileOutputStream(file)));
            FileStream fs = file.OpenWrite();
            BinaryWriter dos = new BinaryWriter(fs);
            try
            {
                writeHeader(dos);
            }
            finally
            {
                dos.Close();
            }
        }


        public virtual void writeHeader(/*DataOutput*/ BinaryWriter dos)
        {
            writeFloat(dos, delta);
            writeFloat(dos, depmin);
            writeFloat(dos, depmax);
            writeFloat(dos, scale);
            writeFloat(dos, odelta);
            writeFloat(dos, b);
            writeFloat(dos, e);
            writeFloat(dos, o);
            writeFloat(dos, a);
            writeFloat(dos, fmt);
            writeFloat(dos, t0);
            writeFloat(dos, t1);
            writeFloat(dos, t2);
            writeFloat(dos, t3);
            writeFloat(dos, t4);
            writeFloat(dos, t5);
            writeFloat(dos, t6);
            writeFloat(dos, t7);
            writeFloat(dos, t8);
            writeFloat(dos, t9);
            writeFloat(dos, f);
            writeFloat(dos, resp0);
            writeFloat(dos, resp1);
            writeFloat(dos, resp2);
            writeFloat(dos, resp3);
            writeFloat(dos, resp4);
            writeFloat(dos, resp5);
            writeFloat(dos, resp6);
            writeFloat(dos, resp7);
            writeFloat(dos, resp8);
            writeFloat(dos, resp9);
            writeFloat(dos, stla);
            writeFloat(dos, stlo);
            writeFloat(dos, stel);
            writeFloat(dos, stdp);
            writeFloat(dos, evla);
            writeFloat(dos, evlo);
            writeFloat(dos, evel);
            writeFloat(dos, evdp);
            writeFloat(dos, mag);
            writeFloat(dos, user0);
            writeFloat(dos, user1);
            writeFloat(dos, user2);
            writeFloat(dos, user3);
            writeFloat(dos, user4);
            writeFloat(dos, user5);
            writeFloat(dos, user6);
            writeFloat(dos, user7);
            writeFloat(dos, user8);
            writeFloat(dos, user9);
            writeFloat(dos, dist);
            writeFloat(dos, az);
            writeFloat(dos, baz);
            writeFloat(dos, gcarc);
            writeFloat(dos, sb);
            writeFloat(dos, sdelta);
            writeFloat(dos, depmen);
            writeFloat(dos, cmpaz);
            writeFloat(dos, cmpinc);
            writeFloat(dos, xminimum);
            writeFloat(dos, xmaximum);
            writeFloat(dos, yminimum);
            writeFloat(dos, ymaximum);
            writeFloat(dos, unused6);
            writeFloat(dos, unused7);
            writeFloat(dos, unused8);
            writeFloat(dos, unused9);
            writeFloat(dos, unused10);
            writeFloat(dos, unused11);
            writeFloat(dos, unused12);
            writeInt(dos, nzyear);
            writeInt(dos, nzjday);
            writeInt(dos, nzhour);
            writeInt(dos, nzmin);
            writeInt(dos, nzsec);
            writeInt(dos, nzmsec);
            writeInt(dos, nvhdr);
            writeInt(dos, norid);
            writeInt(dos, nevid);
            writeInt(dos, npts);
            writeInt(dos, nsnpts);
            writeInt(dos, nwfid);
            writeInt(dos, nxsize);
            writeInt(dos, nysize);
            writeInt(dos, unused15);
            writeInt(dos, iftype);
            writeInt(dos, idep);
            writeInt(dos, iztype);
            writeInt(dos, unused16);
            writeInt(dos, iinst);
            writeInt(dos, istreg);
            writeInt(dos, ievreg);
            writeInt(dos, ievtyp);
            writeInt(dos, iqual);
            writeInt(dos, isynth);
            writeInt(dos, imagtyp);
            writeInt(dos, imagsrc);
            writeInt(dos, unused19);
            writeInt(dos, unused20);
            writeInt(dos, unused21);
            writeInt(dos, unused22);
            writeInt(dos, unused23);
            writeInt(dos, unused24);
            writeInt(dos, unused25);
            writeInt(dos, unused26);
            writeInt(dos, leven);
            writeInt(dos, lpspol);
            writeInt(dos, lovrok);
            writeInt(dos, lcalda);
            writeInt(dos, unused27);
            dos.Write(trimLen(kstnm, 8));
            dos.Write(trimLen(kevnm, 16));
            dos.Write(trimLen(khole, 8));
            dos.Write(trimLen(ko, 8));
            dos.Write(trimLen(ka, 8));
            dos.Write(trimLen(kt0, 8));
            dos.Write(trimLen(kt1, 8));
            dos.Write(trimLen(kt2, 8));
            dos.Write(trimLen(kt3, 8));
            dos.Write(trimLen(kt4, 8));
            dos.Write(trimLen(kt5, 8));
            dos.Write(trimLen(kt6, 8));
            dos.Write(trimLen(kt7, 8));
            dos.Write(trimLen(kt8, 8));
            dos.Write(trimLen(kt9, 8));
            dos.Write(trimLen(kf, 8));
            dos.Write(trimLen(kuser0, 8));
            dos.Write(trimLen(kuser1, 8));
            dos.Write(trimLen(kuser2, 8));
            dos.Write(trimLen(kcmpnm, 8));
            dos.Write(trimLen(knetwk, 8));
            dos.Write(trimLen(kdatrd, 8));
            dos.Write(trimLen(kinst, 8));
        }



        /// <summary>
        /// Sets the byte order when writing to output. Does not change the internal
        /// representation of the data.
        /// </summary>
        public void setLittleEndian()
        {
            byteOrder = SacConstants.IntelByteOrder;
        }

        /// <summary>
        /// Sets the byte order when writing to output. Does not change the internal
        /// representation of the data.
        /// </summary>
        public void setBigEndian()
        {
            byteOrder = SacConstants.SunByteOrder;
        }

        public static short swapBytes(short val)
        {
            return (short)(((val & 0xff00) >> 8) + ((val & 0x00ff) << 8));
        }

        public static int swapBytes(int val)
        {
            return (int)((long)((ulong)(val & 0xff000000) >> 24)) + ((val & 0x00ff0000) >> 8) + ((val & 0x0000ff00) << 8) + ((val & 0x000000ff) << 24);

        }

        public static long swapBytes(long val)
        {
            return ((long)((ulong)(val & 0xffL << 56) >> 56)) + ((val & 0xffL << 48) >> 40) + ((val & 0xffL << 40) >> 24) + ((val & 0xffL << 32) >> 8) + ((val & 0xffL << 24) << 8) + ((val & 0xffL << 16) << 24) + ((val & 0xffL << 8) << 40) + ((val & 0xffL) << 56);
        }




        public static string trimLen(string s, int len)
        {
            if (s.Length > len)
            {
                s = s.Substring(0, len - 1);
            }
            while (s.Length < len)
            {
                s += " ";
            }
            return s;
        }

        //private static readonly ThreadLocal<DecimalFormat> decimalFormat = new ThreadLocalAnonymousInnerClass();

        //private class ThreadLocalAnonymousInnerClass : ThreadLocal<DecimalFormat>
        //{
        //    protected internal override DecimalFormat initialValue()
        //    {
        //        return (new DecimalFormat("#####.####", new DecimalFormatSymbols(Locale.US)));
        //    }
        //}

        public static string format(string label, float f)
        {
            return format(label, /*decimalFormat.get().format(f)*/f.ToString("#####.####"), 10, 8);
        }

        public static string format(string label, string val, int labelWidth, int valWidth)
        {
            while (label.Length < labelWidth)
            {
                label = " " + label;
            }
            while (val.Length < valWidth)
            {
                val = " " + val;
            }
            return label + " = " + val;
        }

        public static string formatLine(string s1, float f1, string s2, float f2, string s3, float f3, string s4, float f4, string s5, float f5)
        {
            return format(s1, f1) + format(s2, f2) + format(s3, f3) + format(s4, f4) + format(s5, f5);
        }

        public virtual void printHeader(string filename)
        {
            System.IO.TextWriter writeFile = new StreamWriter(filename);
            printHeader(writeFile);
            writeFile.Flush();
            writeFile.Close();


        }


        public virtual void printHeader(TextWriter @out)
        {
            @out.WriteLine(formatLine("delta", delta, "depmin", depmin, "depmax", depmax, "scale", scale, "odelta", odelta));
            @out.WriteLine(formatLine("b", b, "e", e, "o", o, "a", a, "fmt", fmt));
            @out.WriteLine(formatLine("t0", t0, "t1", t1, "t2", t2, "t3", t3, "t4", t4));
            @out.WriteLine(formatLine("t5", t5, "t6", t6, "t7", t7, "t8", t8, "t9", t9));
            @out.WriteLine(formatLine("f", f, "resp0", resp0, "resp1", resp1, "resp2", resp2, "resp3", resp3));
            @out.WriteLine(formatLine("resp4", resp4, "resp5", resp5, "resp6", resp6, "resp7", resp7, "resp8", resp8));
            @out.WriteLine(formatLine("resp9", resp9, "stla", stla, "stlo", stlo, "stel", stel, "stdp", stdp));
            @out.WriteLine(formatLine("evla", evla, "evlo", evlo, "evel", evel, "evdp", evdp, "mag", mag));
            @out.WriteLine(formatLine("user0", user0, "user1", user1, "user2", user2, "user3", user3, "user4", user4));
            @out.WriteLine(formatLine("user5", user5, "user6", user6, "user7", user7, "user8", user8, "user9", user9));
            @out.WriteLine(formatLine("dist", dist, "az", az, "baz", baz, "gcarc", gcarc, "sb", sb));
            @out.WriteLine(formatLine("sdelta", sdelta, "depmen", depmen, "cmpaz", cmpaz, "cmpinc", cmpinc, "xminimum", xminimum));
            @out.WriteLine(formatLine("xmaximum", xmaximum, "yminimum", yminimum, "ymaximum", ymaximum, "unused6", unused6, "unused7", unused7));
            @out.WriteLine(formatLine("unused8", unused8, "unused9", unused9, "unused10", unused10, "unused11", unused11, "unused12", unused12));
            @out.WriteLine(formatLine("nzyear", nzyear, "nzjday", nzjday, "nzhour", nzhour, "nzmin", nzmin, "nzsec", nzsec));
            @out.WriteLine(formatLine("nzmsec", nzmsec, "nvhdr", nvhdr, "norid", norid, "nevid", nevid, "npts", npts));
            @out.WriteLine(formatLine("nsnpts", nsnpts, "nwfid", nwfid, "nxsize", nxsize, "nysize", nysize, "unused15", unused15));
            @out.WriteLine(formatLine("iftype", iftype, "idep", idep, "iztype", iztype, "unused16", unused16, "iinst", iinst));
            @out.WriteLine(formatLine("istreg", istreg, "ievreg", ievreg, "ievtyp", ievtyp, "iqual", iqual, "isynth", isynth));
            @out.WriteLine(formatLine("imagtyp", imagtyp, "imagsrc", imagsrc, "unused19", unused19, "unused20", unused20, "unused21", unused21));
            @out.WriteLine(formatLine("unused22", unused22, "unused23", unused23, "unused24", unused24, "unused25", unused25, "unused26", unused26));
            @out.WriteLine(formatLine("leven", leven, "lpspol", lpspol, "lovrok", lovrok, "lcalda", lcalda, "unused27", unused27));
            int labelWidth = 10;
            int wideValWidth = 31;
            int valWidth = 10;
            @out.WriteLine(format("kstnm", kstnm, labelWidth, valWidth) + format("kevnm", kevnm, labelWidth, wideValWidth) + format("khole", khole, labelWidth + 2, valWidth));
            @out.WriteLine(format("ko", ko, labelWidth, valWidth) + format("ka = ", ka, labelWidth, valWidth) + format("kt0", kt0, labelWidth, valWidth) + format("kt1", kt1, labelWidth, valWidth));
            @out.WriteLine(format("kt2", kt2, labelWidth, valWidth) + format("kt3 = ", kt3, labelWidth, valWidth) + format("kt4", kt4, labelWidth, valWidth) + format("kt5", kt5, labelWidth, valWidth));
            @out.WriteLine(format("kt6", kt6, labelWidth, valWidth) + format("kt7 = ", kt7, labelWidth, valWidth) + format("kt8", kt8, labelWidth, valWidth) + format("kt9", kt9, labelWidth, valWidth));
            @out.WriteLine(format("kf", kf, labelWidth, valWidth) + format("kuser0 = ", kuser0, labelWidth, valWidth) + format("kuser1", kuser1, labelWidth, valWidth) + format("kuser2", kuser2, labelWidth, valWidth));
            @out.WriteLine(format("kcmpnm", kcmpnm, labelWidth, valWidth) + format("knetwk = ", knetwk, labelWidth, valWidth) + format("kdatrd", kdatrd, labelWidth, valWidth) + format("kinst", kinst, labelWidth, valWidth));
        }


        internal bool byteOrder = SacConstants.SunByteOrder;


        public virtual bool ByteOrder
        {
            get
            {
                return byteOrder;
            }
            set
            {
                this.byteOrder = value;
            }
        }


        /// <summary>
        /// RF time increment, sec </summary>
        private float delta = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// minimum amplitude </summary>
        private float depmin = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// maximum amplitude </summary>
        private float depmax = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// amplitude scale factor </summary>
        private float scale = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// observed time inc </summary>
        private float odelta = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// RD initial time - wrt nz* </summary>
        private float b = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// RD end time </summary>
        private float e = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// event start </summary>
        private float o = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// 1st arrival time </summary>
        private float a = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// internal use </summary>
        private float fmt = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t0 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t1 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t2 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t3 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t4 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t5 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t6 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t7 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t8 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// user-defined time pick </summary>
        private float t9 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// event end, sec > 0 </summary>
        private float f = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp0 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp1 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp2 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp3 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp4 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp5 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp6 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp7 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp8 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// instrument respnse parm </summary>
        private float resp9 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// T station latititude </summary>
        private float stla = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// T station longitude </summary>
        private float stlo = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// T station elevation, m </summary>
        private float stel = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// T station depth, m </summary>
        private float stdp = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// event latitude </summary>
        private float evla = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// event longitude </summary>
        private float evlo = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// event elevation </summary>
        private float evel = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// event depth </summary>
        private float evdp = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// magnitude value </summary>
        private float mag = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user0 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user1 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user2 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user3 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user4 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user5 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user6 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user7 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user8 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// available to user </summary>
        private float user9 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// stn-event distance, km </summary>
        private float dist = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// event-stn azimuth </summary>
        private float az = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// stn-event azimuth </summary>
        private float baz = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// stn-event dist, degrees </summary>
        private float gcarc = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// saved b value </summary>
        private float sb = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// saved delta value </summary>
        private float sdelta = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// mean value, amplitude </summary>
        private float depmen = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// T component azimuth </summary>
        private float cmpaz = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// T component inclination </summary>
        private float cmpinc = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// XYZ X minimum value </summary>
        private float xminimum = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// XYZ X maximum value </summary>
        private float xmaximum = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// XYZ Y minimum value </summary>
        private float yminimum = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// XYZ Y maximum value </summary>
        private float ymaximum = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private float unused6 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private float unused7 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private float unused8 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private float unused9 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private float unused10 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private float unused11 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private float unused12 = SacConstants.FLOAT_UNDEF;

        /// <summary>
        /// F zero time of file, yr </summary>
        private int nzyear = SacConstants.INT_UNDEF;

        /// <summary>
        /// F zero time of file, day </summary>
        private int nzjday = SacConstants.INT_UNDEF;

        /// <summary>
        /// F zero time of file, hr </summary>
        private int nzhour = SacConstants.INT_UNDEF;

        /// <summary>
        /// F zero time of file, min </summary>
        private int nzmin = SacConstants.INT_UNDEF;

        /// <summary>
        /// F zero time of file, sec </summary>
        private int nzsec = SacConstants.INT_UNDEF;

        /// <summary>
        /// F zero time of file, msec </summary>
        private int nzmsec = SacConstants.INT_UNDEF;

        /// <summary>
        /// R header version number </summary>
        private int nvhdr = SacConstants.DEFAULT_NVHDR;

        /// <summary>
        /// Origin ID </summary>
        private int norid = SacConstants.INT_UNDEF;

        /// <summary>
        /// Event ID </summary>
        private int nevid = SacConstants.INT_UNDEF;

        /// <summary>
        /// RF number of samples </summary>
        private int npts = SacConstants.INT_UNDEF;

        /// <summary>
        /// saved npts </summary>
        private int nsnpts = SacConstants.INT_UNDEF;

        /// <summary>
        /// Waveform ID </summary>
        private int nwfid = SacConstants.INT_UNDEF;

        /// <summary>
        /// XYZ X size </summary>
        private int nxsize = SacConstants.INT_UNDEF;

        /// <summary>
        /// XYZ Y size </summary>
        private int nysize = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused15 = SacConstants.INT_UNDEF;

        /// <summary>
        /// RA type of file </summary>
        private int iftype = SacConstants.INT_UNDEF;

        /// <summary>
        /// type of amplitude </summary>
        private int idep = SacConstants.INT_UNDEF;

        /// <summary>
        /// zero time equivalence </summary>
        private int iztype = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused16 = SacConstants.INT_UNDEF;

        /// <summary>
        /// recording instrument </summary>
        private int iinst = SacConstants.INT_UNDEF;

        /// <summary>
        /// stn geographic region </summary>
        private int istreg = SacConstants.INT_UNDEF;

        /// <summary>
        /// event geographic region </summary>
        private int ievreg = SacConstants.INT_UNDEF;

        /// <summary>
        /// event type </summary>
        private int ievtyp = SacConstants.INT_UNDEF;

        /// <summary>
        /// quality of data </summary>
        private int iqual = SacConstants.INT_UNDEF;

        /// <summary>
        /// synthetic data flag </summary>
        private int isynth = SacConstants.INT_UNDEF;

        /// <summary>
        /// magnitude type </summary>
        private int imagtyp = SacConstants.INT_UNDEF;

        /// <summary>
        /// magnitude source </summary>
        private int imagsrc = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused19 = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused20 = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused21 = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused22 = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused23 = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused24 = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused25 = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused26 = SacConstants.INT_UNDEF;

        /// <summary>
        /// RA data-evenly-spaced flag </summary>
        private int leven = SacConstants.INT_UNDEF;

        /// <summary>
        /// station polarity flag </summary>
        private int lpspol = SacConstants.INT_UNDEF;

        /// <summary>
        /// overwrite permission </summary>
        private int lovrok = SacConstants.INT_UNDEF;

        /// <summary>
        /// calc distance, azimuth </summary>
        private int lcalda = SacConstants.INT_UNDEF;

        /// <summary>
        /// reserved for future use </summary>
        private int unused27 = SacConstants.INT_UNDEF;

        /// <summary>
        /// F station name </summary>
        private string kstnm = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// event name </summary>
        private string kevnm = SacConstants.STRING16_UNDEF;

        /// <summary>
        /// man-made event name </summary>
        private string khole = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// event origin time id </summary>
        private string ko = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// 1st arrival time ident </summary>
        private string ka = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 0 ident </summary>
        private string kt0 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 1 ident </summary>
        private string kt1 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 2 ident </summary>
        private string kt2 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 3 ident </summary>
        private string kt3 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 4 ident </summary>
        private string kt4 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 5 ident </summary>
        private string kt5 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 6 ident </summary>
        private string kt6 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 7 ident </summary>
        private string kt7 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 8 ident </summary>
        private string kt8 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// time pick 9 ident </summary>
        private string kt9 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// end of event ident </summary>
        private string kf = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// available to user </summary>
        private string kuser0 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// available to user </summary>
        private string kuser1 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// available to user </summary>
        private string kuser2 = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// F component name </summary>
        private string kcmpnm = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// network name </summary>
        private string knetwk = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// date data read </summary>
        private string kdatrd = SacConstants.STRING8_UNDEF;

        /// <summary>
        /// instrument name </summary>
        private string kinst = SacConstants.STRING8_UNDEF;



        public virtual float Delta
        {
            get
            {
                return delta;
            }
            set
            {
                this.delta = value;
            }
        }




        public virtual float Depmin
        {
            get
            {
                return depmin;
            }
            set
            {
                this.depmin = value;
            }
        }




        public virtual float Depmax
        {
            get
            {
                return depmax;
            }
            set
            {
                this.depmax = value;
            }
        }




        public virtual float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                this.scale = value;
            }
        }




        public virtual float Odelta
        {
            get
            {
                return odelta;
            }
            set
            {
                this.odelta = value;
            }
        }




        public virtual float B
        {
            get
            {
                return b;
            }
            set
            {
                this.b = value;
            }
        }




        public virtual float E
        {
            get
            {
                return e;
            }
            set
            {
                this.e = value;
            }
        }




        public virtual float O
        {
            get
            {
                return o;
            }
            set
            {
                this.o = value;
            }
        }




        public virtual float A
        {
            get
            {
                return a;
            }
            set
            {
                this.a = value;
            }
        }




        public virtual float Fmt
        {
            get
            {
                return fmt;
            }
            set
            {
                this.fmt = value;
            }
        }



        public virtual float getTHeader(int index)
        {
            switch (index)
            {
                case 0:
                    return T0;
                case 1:
                    return T1;
                case 2:
                    return T2;
                case 3:
                    return T3;
                case 4:
                    return T4;
                case 5:
                    return T5;
                case 6:
                    return T6;
                case 7:
                    return T7;
                case 8:
                    return T8;
                case 9:
                    return T9;
                default:
                    throw new System.ArgumentException("Illegal T header index, " + index + ", must be 0-9");
            }
        }


        public virtual void setTHeader(int index, float val)
        {
            switch (index)
            {
                case 0:
                    T0 = val;
                    break;
                case 1:
                    T1 = val;
                    break;
                case 2:
                    T2 = val;
                    break;
                case 3:
                    T3 = val;
                    break;
                case 4:
                    T4 = val;
                    break;
                case 5:
                    T5 = val;
                    break;
                case 6:
                    T6 = val;
                    break;
                case 7:
                    T7 = val;
                    break;
                case 8:
                    T8 = val;
                    break;
                case 9:
                    T9 = val;
                    break;
                default:
                    throw new System.ArgumentException("Illegal T header index, " + index + ", must be 0-9");
            }
        }

        /// <summary>
        /// Sets T header specified by the index to val, and sets the corresponding
        /// KT header to be the label. indices 0-9 map to T0-T9 and index 10 maps to 
        /// the A header.
        /// </summary>
        public virtual void setTHeader(int index, float val, string kLabel)
        {
            switch (index)
            {
                case 0:
                    T0 = val;
                    Kt0 = kLabel;
                    break;
                case 1:
                    T1 = val;
                    Kt1 = kLabel;
                    break;
                case 2:
                    T2 = val;
                    Kt2 = kLabel;
                    break;
                case 3:
                    T3 = val;
                    Kt3 = kLabel;
                    break;
                case 4:
                    T4 = val;
                    Kt4 = kLabel;
                    break;
                case 5:
                    T5 = val;
                    Kt5 = kLabel;
                    break;
                case 6:
                    T6 = val;
                    Kt6 = kLabel;
                    break;
                case 7:
                    T7 = val;
                    Kt7 = kLabel;
                    break;
                case 8:
                    T8 = val;
                    Kt8 = kLabel;
                    break;
                case 9:
                    T9 = val;
                    Kt9 = kLabel;
                    break;
                default:
                    throw new System.ArgumentException("Illegal T header index, " + index + ", must be 0-9");
            }
        }


        public virtual string getKTHeader(int index)
        {
            switch (index)
            {
                case 0:
                    return Kt0;
                case 1:
                    return Kt1;
                case 2:
                    return Kt2;
                case 3:
                    return Kt3;
                case 4:
                    return Kt4;
                case 5:
                    return Kt5;
                case 6:
                    return Kt6;
                case 7:
                    return Kt7;
                case 8:
                    return Kt8;
                case 9:
                    return Kt9;
                default:
                    throw new System.ArgumentException("Illegal T header index, " + index + ", must be 0-9");
            }
        }

        public virtual void setKtHeader(int index, string val)
        {
            switch (index)
            {
                case 0:
                    Kt0 = val;
                    break;
                case 1:
                    Kt1 = val;
                    break;
                case 2:
                    Kt2 = val;
                    break;
                case 3:
                    Kt3 = val;
                    break;
                case 4:
                    Kt4 = val;
                    break;
                case 5:
                    Kt5 = val;
                    break;
                case 6:
                    Kt6 = val;
                    break;
                case 7:
                    Kt7 = val;
                    break;
                case 8:
                    Kt8 = val;
                    break;
                case 9:
                    Kt9 = val;
                    break;
                default:
                    throw new System.ArgumentException("Illegal T header index, " + index + ", must be 0-9");
            }
        }


        public virtual float T0
        {
            get
            {
                return t0;
            }
            set
            {
                this.t0 = value;
            }
        }




        public virtual float T1
        {
            get
            {
                return t1;
            }
            set
            {
                this.t1 = value;
            }
        }




        public virtual float T2
        {
            get
            {
                return t2;
            }
            set
            {
                this.t2 = value;
            }
        }




        public virtual float T3
        {
            get
            {
                return t3;
            }
            set
            {
                this.t3 = value;
            }
        }




        public virtual float T4
        {
            get
            {
                return t4;
            }
            set
            {
                this.t4 = value;
            }
        }




        public virtual float T5
        {
            get
            {
                return t5;
            }
            set
            {
                this.t5 = value;
            }
        }




        public virtual float T6
        {
            get
            {
                return t6;
            }
            set
            {
                this.t6 = value;
            }
        }




        public virtual float T7
        {
            get
            {
                return t7;
            }
            set
            {
                this.t7 = value;
            }
        }




        public virtual float T8
        {
            get
            {
                return t8;
            }
            set
            {
                this.t8 = value;
            }
        }




        public virtual float T9
        {
            get
            {
                return t9;
            }
            set
            {
                this.t9 = value;
            }
        }




        public virtual float F
        {
            get
            {
                return f;
            }
            set
            {
                this.f = value;
            }
        }




        public virtual float Resp0
        {
            get
            {
                return resp0;
            }
            set
            {
                this.resp0 = value;
            }
        }




        public virtual float Resp1
        {
            get
            {
                return resp1;
            }
            set
            {
                this.resp1 = value;
            }
        }




        public virtual float Resp2
        {
            get
            {
                return resp2;
            }
            set
            {
                this.resp2 = value;
            }
        }




        public virtual float Resp3
        {
            get
            {
                return resp3;
            }
            set
            {
                this.resp3 = value;
            }
        }




        public virtual float Resp4
        {
            get
            {
                return resp4;
            }
            set
            {
                this.resp4 = value;
            }
        }

        public virtual float Resp5
        {
            get
            {
                return resp5;
            }
            set
            {
                this.resp5 = value;
            }
        }




        public virtual float Resp6
        {
            get
            {
                return resp6;
            }
            set
            {
                this.resp6 = value;
            }
        }




        public virtual float Resp7
        {
            get
            {
                return resp7;
            }
            set
            {
                this.resp7 = value;
            }
        }




        public virtual float Resp8
        {
            get
            {
                return resp8;
            }
            set
            {
                this.resp8 = value;
            }
        }




        public virtual float Resp9
        {
            get
            {
                return resp9;
            }
            set
            {
                this.resp9 = value;
            }
        }




        public virtual float Stla
        {
            get
            {
                return stla;
            }
            set
            {
                this.stla = value;
            }
        }




        public virtual float Stlo
        {
            get
            {
                return stlo;
            }
            set
            {
                this.stlo = value;
            }
        }




        public virtual float Stel
        {
            get
            {
                return stel;
            }
            set
            {
                this.stel = value;
            }
        }




        public virtual float Stdp
        {
            get
            {
                return stdp;
            }
            set
            {
                this.stdp = value;
            }
        }




        public virtual float Evla
        {
            get
            {
                return evla;
            }
            set
            {
                this.evla = value;
            }
        }




        public virtual float Evlo
        {
            get
            {
                return evlo;
            }
            set
            {
                this.evlo = value;
            }
        }




        public virtual float Evel
        {
            get
            {
                return evel;
            }
            set
            {
                this.evel = value;
            }
        }




        public virtual float Evdp
        {
            get
            {
                return evdp;
            }
            set
            {
                this.evdp = value;
            }
        }




        public virtual float Mag
        {
            get
            {
                return mag;
            }
            set
            {
                this.mag = value;
            }
        }



        public virtual float getUserHeader(int index)
        {
            return index switch
            {
                0 => User0,
                1 => User1,
                2 => User2,
                3 => User3,
                4 => User4,
                5 => User5,
                6 => User6,
                7 => User7,
                8 => User8,
                9 => User9,
                _ => throw new System.ArgumentException("Illegal User header index, " + index + ", must be 0-9"),
            };
        }

        public virtual void setUserHeader(int index, float val)
        {
            switch (index)
            {
                case 0:
                    User0 = val;
                    break;
                case 1:
                    User1 = val;
                    break;
                case 2:
                    User2 = val;
                    break;
                case 3:
                    User3 = val;
                    break;
                case 4:
                    User4 = val;
                    break;
                case 5:
                    User5 = val;
                    break;
                case 6:
                    User6 = val;
                    break;
                case 7:
                    User7 = val;
                    break;
                case 8:
                    User8 = val;
                    break;
                case 9:
                    User9 = val;
                    break;
                default:
                    throw new System.ArgumentException("Illegal User header index, " + index + ", must be 0-9");
            }
        }


        public virtual float User0
        {
            get
            {
                return user0;
            }
            set
            {
                this.user0 = value;
            }
        }




        public virtual float User1
        {
            get
            {
                return user1;
            }
            set
            {
                this.user1 = value;
            }
        }




        public virtual float User2
        {
            get
            {
                return user2;
            }
            set
            {
                this.user2 = value;
            }
        }




        public virtual float User3
        {
            get
            {
                return user3;
            }
            set
            {
                this.user3 = value;
            }
        }




        public virtual float User4
        {
            get
            {
                return user4;
            }
            set
            {
                this.user4 = value;
            }
        }




        public virtual float User5
        {
            get
            {
                return user5;
            }
            set
            {
                this.user5 = value;
            }
        }




        public virtual float User6
        {
            get
            {
                return user6;
            }
            set
            {
                this.user6 = value;
            }
        }




        public virtual float User7
        {
            get
            {
                return user7;
            }
            set
            {
                this.user7 = value;
            }
        }




        public virtual float User8
        {
            get
            {
                return user8;
            }
            set
            {
                this.user8 = value;
            }
        }




        public virtual float User9
        {
            get
            {
                return user9;
            }
            set
            {
                this.user9 = value;
            }
        }




        public virtual float Dist
        {
            get
            {
                return dist;
            }
            set
            {
                this.dist = value;
            }
        }




        public virtual float Az
        {
            get
            {
                return az;
            }
            set
            {
                this.az = value;
            }
        }




        public virtual float Baz
        {
            get
            {
                return baz;
            }
            set
            {
                this.baz = value;
            }
        }




        public virtual float Gcarc
        {
            get
            {
                return gcarc;
            }
            set
            {
                this.gcarc = value;
            }
        }




        public virtual float Sb
        {
            get
            {
                return sb;
            }
            set
            {
                this.sb = value;
            }
        }




        public virtual float Sdelta
        {
            get
            {
                return sdelta;
            }
            set
            {
                this.sdelta = value;
            }
        }

        public virtual float Depmen
        {
            get
            {
                return depmen;
            }
            set
            {
                this.depmen = value;
            }
        }




        public virtual float Cmpaz
        {
            get
            {
                return cmpaz;
            }
            set
            {
                this.cmpaz = value;
            }
        }




        public virtual float Cmpinc
        {
            get
            {
                return cmpinc;
            }
            set
            {
                this.cmpinc = value;
            }
        }




        public virtual float Xminimum
        {
            get
            {
                return xminimum;
            }
            set
            {
                this.xminimum = value;
            }
        }




        public virtual float Xmaximum
        {
            get
            {
                return xmaximum;
            }
            set
            {
                this.xmaximum = value;
            }
        }




        public virtual float Yminimum
        {
            get
            {
                return yminimum;
            }
            set
            {
                this.yminimum = value;
            }
        }




        public virtual float Ymaximum
        {
            get
            {
                return ymaximum;
            }
            set
            {
                this.ymaximum = value;
            }
        }




        public virtual float Unused6
        {
            get
            {
                return unused6;
            }
            set
            {
                this.unused6 = value;
            }
        }




        public virtual float Unused7
        {
            get
            {
                return unused7;
            }
            set
            {
                this.unused7 = value;
            }
        }




        public virtual float Unused8
        {
            get
            {
                return unused8;
            }
            set
            {
                this.unused8 = value;
            }
        }




        public virtual float Unused9
        {
            get
            {
                return unused9;
            }
            set
            {
                this.unused9 = value;
            }
        }




        public virtual float Unused10
        {
            get
            {
                return unused10;
            }
            set
            {
                this.unused10 = value;
            }
        }




        public virtual float Unused11
        {
            get
            {
                return unused11;
            }
            set
            {
                this.unused11 = value;
            }
        }




        public virtual float Unused12
        {
            get
            {
                return unused12;
            }
            set
            {
                this.unused12 = value;
            }
        }




        public virtual int Nzyear
        {
            get
            {
                return nzyear;
            }
            set
            {
                this.nzyear = value;
            }
        }




        public virtual int Nzjday
        {
            get
            {
                return nzjday;
            }
            set
            {
                this.nzjday = value;
            }
        }

        public virtual int Nzhour
        {
            get
            {
                return nzhour;
            }
            set
            {
                this.nzhour = value;
            }
        }




        public virtual int Nzmin
        {
            get
            {
                return nzmin;
            }
            set
            {
                this.nzmin = value;
            }
        }




        public virtual int Nzsec
        {
            get
            {
                return nzsec;
            }
            set
            {
                this.nzsec = value;
            }
        }




        public virtual int Nzmsec
        {
            get
            {
                return nzmsec;
            }
            set
            {
                this.nzmsec = value;
            }
        }




        public virtual int Nvhdr
        {
            get
            {
                return nvhdr;
            }
            set
            {
                this.nvhdr = value;
            }
        }




        public virtual int Norid
        {
            get
            {
                return norid;
            }
            set
            {
                this.norid = value;
            }
        }




        public virtual int Nevid
        {
            get
            {
                return nevid;
            }
            set
            {
                this.nevid = value;
            }
        }




        public virtual int Npts
        {
            get
            {
                return npts;
            }
            set
            {
                this.npts = value;
            }
        }




        public virtual int Nsnpts
        {
            get
            {
                return nsnpts;
            }
            set
            {
                this.nsnpts = value;
            }
        }




        public virtual int Nwfid
        {
            get
            {
                return nwfid;
            }
            set
            {
                this.nwfid = value;
            }
        }




        public virtual int Nxsize
        {
            get
            {
                return nxsize;
            }
            set
            {
                this.nxsize = value;
            }
        }




        public virtual int Nysize
        {
            get
            {
                return nysize;
            }
            set
            {
                this.nysize = value;
            }
        }




        public virtual int Unused15
        {
            get
            {
                return unused15;
            }
            set
            {
                this.unused15 = value;
            }
        }




        public virtual int Iftype
        {
            get
            {
                return iftype;
            }
            set
            {
                this.iftype = value;
            }
        }




        public virtual int Idep
        {
            get
            {
                return idep;
            }
            set
            {
                this.idep = value;
            }
        }




        public virtual int Iztype
        {
            get
            {
                return iztype;
            }
            set
            {
                this.iztype = value;
            }
        }

        public virtual int Unused16
        {
            get
            {
                return unused16;
            }
            set
            {
                this.unused16 = value;
            }
        }




        public virtual int Iinst
        {
            get
            {
                return iinst;
            }
            set
            {
                this.iinst = value;
            }
        }




        public virtual int Istreg
        {
            get
            {
                return istreg;
            }
            set
            {
                this.istreg = value;
            }
        }




        public virtual int Ievreg
        {
            get
            {
                return ievreg;
            }
            set
            {
                this.ievreg = value;
            }
        }




        public virtual int Ievtyp
        {
            get
            {
                return ievtyp;
            }
            set
            {
                this.ievtyp = value;
            }
        }




        public virtual int Iqual
        {
            get
            {
                return iqual;
            }
            set
            {
                this.iqual = value;
            }
        }




        public virtual int Isynth
        {
            get
            {
                return isynth;
            }
            set
            {
                this.isynth = value;
            }
        }




        public virtual int Imagtyp
        {
            get
            {
                return imagtyp;
            }
            set
            {
                this.imagtyp = value;
            }
        }




        public virtual int Imagsrc
        {
            get
            {
                return imagsrc;
            }
            set
            {
                this.imagsrc = value;
            }
        }




        public virtual int Unused19
        {
            get
            {
                return unused19;
            }
            set
            {
                this.unused19 = value;
            }
        }




        public virtual int Unused20
        {
            get
            {
                return unused20;
            }
            set
            {
                this.unused20 = value;
            }
        }




        public virtual int Unused21
        {
            get
            {
                return unused21;
            }
            set
            {
                this.unused21 = value;
            }
        }




        public virtual int Unused22
        {
            get
            {
                return unused22;
            }
            set
            {
                this.unused22 = value;
            }
        }




        public virtual int Unused23
        {
            get
            {
                return unused23;
            }
            set
            {
                this.unused23 = value;
            }
        }




        public virtual int Unused24
        {
            get
            {
                return unused24;
            }
            set
            {
                this.unused24 = value;
            }
        }




        public virtual int Unused25
        {
            get
            {
                return unused25;
            }
            set
            {
                this.unused25 = value;
            }
        }

        public virtual int Unused26
        {
            get
            {
                return unused26;
            }
            set
            {
                this.unused26 = value;
            }
        }




        public virtual int Leven
        {
            get
            {
                return leven;
            }
            set
            {
                this.leven = value;
            }
        }




        public virtual int Lpspol
        {
            get
            {
                return lpspol;
            }
            set
            {
                this.lpspol = value;
            }
        }




        public virtual int Lovrok
        {
            get
            {
                return lovrok;
            }
            set
            {
                this.lovrok = value;
            }
        }




        public virtual int Lcalda
        {
            get
            {
                return lcalda;
            }
            set
            {
                this.lcalda = value;
            }
        }




        public virtual int Unused27
        {
            get
            {
                return unused27;
            }
            set
            {
                this.unused27 = value;
            }
        }




        public virtual string Kstnm
        {
            get
            {
                return kstnm;
            }
            set
            {
                this.kstnm = value;
            }
        }




        public virtual string Kevnm
        {
            get
            {
                return kevnm;
            }
            set
            {
                this.kevnm = value;
            }
        }




        public virtual string Khole
        {
            get
            {
                return khole;
            }
            set
            {
                this.khole = value;
            }
        }




        public virtual string Ko
        {
            get
            {
                return ko;
            }
            set
            {
                this.ko = value;
            }
        }




        public virtual string Ka
        {
            get
            {
                return ka;
            }
            set
            {
                this.ka = value;
            }
        }




        public virtual string Kt0
        {
            get
            {
                return kt0;
            }
            set
            {
                this.kt0 = value;
            }
        }




        public virtual string Kt1
        {
            get
            {
                return kt1;
            }
            set
            {
                this.kt1 = value;
            }
        }




        public virtual string Kt2
        {
            get
            {
                return kt2;
            }
            set
            {
                this.kt2 = value;
            }
        }




        public virtual string Kt3
        {
            get
            {
                return kt3;
            }
            set
            {
                this.kt3 = value;
            }
        }




        public virtual string Kt4
        {
            get
            {
                return kt4;
            }
            set
            {
                this.kt4 = value;
            }
        }

        public virtual string Kt5
        {
            get
            {
                return kt5;
            }
            set
            {
                this.kt5 = value;
            }
        }




        public virtual string Kt6
        {
            get
            {
                return kt6;
            }
            set
            {
                this.kt6 = value;
            }
        }




        public virtual string Kt7
        {
            get
            {
                return kt7;
            }
            set
            {
                this.kt7 = value;
            }
        }




        public virtual string Kt8
        {
            get
            {
                return kt8;
            }
            set
            {
                this.kt8 = value;
            }
        }




        public virtual string Kt9
        {
            get
            {
                return kt9;
            }
            set
            {
                this.kt9 = value;
            }
        }




        public virtual string Kf
        {
            get
            {
                return kf;
            }
            set
            {
                this.kf = value;
            }
        }




        public virtual string Kuser0
        {
            get
            {
                return kuser0;
            }
            set
            {
                this.kuser0 = value;
            }
        }




        public virtual string Kuser1
        {
            get
            {
                return kuser1;
            }
            set
            {
                this.kuser1 = value;
            }
        }




        public virtual string Kuser2
        {
            get
            {
                return kuser2;
            }
            set
            {
                this.kuser2 = value;
            }
        }




        public virtual string Kcmpnm
        {
            get
            {
                return kcmpnm;
            }
            set
            {
                this.kcmpnm = value;
            }
        }




        public virtual string Knetwk
        {
            get
            {
                return knetwk;
            }
            set
            {
                this.knetwk = value;
            }
        }




        public virtual string Kdatrd
        {
            get
            {
                return kdatrd;
            }
            set
            {
                this.kdatrd = value;
            }
        }




        public virtual string Kinst
        {
            get
            {
                return kinst;
            }
            set
            {
                this.kinst = value;
            }
        }




        private int floatToRawIntBits(float value)
        {
            var buffer = new byte[4];
            BinaryWriter bw = new BinaryWriter(new MemoryStream(buffer));
            bw.Write(value);
            BinaryReader br = new BinaryReader(new MemoryStream(buffer));
            return br.ReadInt32();

        }




    }




}
