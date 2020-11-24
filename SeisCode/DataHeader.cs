using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
    /// <summary>
    /// Container class for SEED Fixed Section Data Header information.
    /// 
    /// @author Philip Crotwell, FISSURES, USC<br>
    ///         Robert Casey, IRIS DMC
    /// @version 08/28/2000
    /// </summary>
    public class DataHeader : ControlHeader
    {

        protected internal sbyte[] stationIdentifier = new sbyte[5];

        protected internal string stationIdentifierString;

        protected internal sbyte[] locationIdentifier = new sbyte[2];

        protected internal string locationIdentifierString;

        protected internal sbyte[] channelIdentifier = new sbyte[3];

        protected internal string channelIdentifierString;

        protected internal sbyte[] networkCode = new sbyte[2];

        protected internal string networkCodeString;

        protected internal sbyte[] startTime = new sbyte[10];

        protected internal int numSamples;

        protected internal int sampleRateFactor;

        protected internal int sampleRateMultiplier;

        protected internal sbyte activityFlags;

        protected internal sbyte ioClockFlags;

        protected internal sbyte dataQualityFlags;

        protected internal sbyte numBlockettes;

        protected internal int timeCorrection;

        protected internal int dataOffset;

        protected internal int dataBlocketteOffset;

        /// <summary>
        /// creates a DataHeader object with listed sequence number, type code, and
        /// continuation code boolean.
        /// </summary>
        /// <param name="sequenceNum">
        ///            sequence number of the record represented by this object. </param>
        /// <param name="typeCode">
        ///            character representing the type of record represented by this
        ///            object </param>
        /// <param name="continuationCode">
        ///            true if this record is flagged as a continuation from its
        ///            previous SEED record </param>
        public DataHeader(int sequenceNum, char typeCode, bool continuationCode) : base(sequenceNum, typeCode, continuationCode)
        {
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
            base.writeASCII(@out, indent);
            @out.print(indent + NetworkCode.Trim() + "." + StationIdentifier.Trim() + "." + LocationIdentifier + "." + ChannelIdentifier);
            @out.print(" start=" + StartTime);
            @out.print(" numPTS=" + getNumSamples());
            @out.print(" sampFac=" + getSampleRateFactor());
            @out.print(" sampMul=" + getSampleRateMultiplier());
            @out.print(" ac=" + ActivityFlags);
            @out.print(" io=" + IOClockFlags);
            @out.print(" qual=" + DataQualityFlags);
            @out.print(" numBlockettes=" + NumBlockettes);
            @out.print(" blocketteOffset=" + getDataBlocketteOffset());
            @out.print(" dataOffset=" + getDataOffset());
            @out.println(" tcor=" + TimeCorrection);
        }

        /// <summary>
        /// Instantiate an object of this class and read an FSDH byte stream into it,
        /// parsing the contents into the instance variables of this object, which
        /// represent the individual FSDH fields.<br>
        /// Note, first 8 bytes are assumed to already have been read.
        /// </summary>
        /// <param name="in">
        ///            SEED data stream offset 8 bytes from beginning of record.
        /// </param>
        /// <param name="sequenceNum">
        ///            6 digit ascii sequence tag at the beginning of
        /// 
        /// SEED record. </param>
        /// <param name="typeCode">
        ///            character representing the type of record being read
        /// </param>
        /// <param name="continuationCode">
        ///            true if this record is flagged as a continuation from its
        ///            previous SEED record.
        /// </param>
        /// <returns> an object of this class with fields filled from 'in' parameter
        /// </returns>
        /// <exception cref="IOException"> </exception>
        /// <exception cref="SeedFormatException"> </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public static DataHeader read(java.io.DataInput in, int sequenceNum, char typeCode, boolean continuationCode) throws java.io.IOException, SeedFormatException
        public static DataHeader read(DataInput @in, int sequenceNum, char typeCode, bool continuationCode)
        {
            sbyte[] buf = new sbyte[40];
            @in.readFully(buf);
            DataHeader data = new DataHeader(sequenceNum, typeCode, continuationCode);
            data.read(buf, 0);
            return data;
        }

        /// <summary>
        /// test whether the data being read needs to be byte-swapped look for bogus
        /// year value to determine this
        /// </summary>
        internal virtual bool flagByteSwap()
        {
            return Btime.shouldSwapBytes(startTime);
        }

        /// <summary>
        /// populates this object with Fixed Section Data Header info. this routine
        /// modified to include byte offset, should the station identifier start at a
        /// byte offset (such as 8 from the beginning of a data record).
        /// </summary>
        /// <param name="buf">
        ///            data buffer containing FSDH information </param>
        /// <param name="offset">
        ///            byte offset to begin reading buf </param>
        protected internal virtual void read(sbyte[] buf, int offset)
        {
            Array.Copy(buf, offset + 0, stationIdentifier, 0, stationIdentifier.Length);
            Array.Copy(buf, offset + 5, locationIdentifier, 0, locationIdentifier.Length);
            Array.Copy(buf, offset + 7, channelIdentifier, 0, channelIdentifier.Length);
            Array.Copy(buf, offset + 10, networkCode, 0, networkCode.Length);
            Array.Copy(buf, offset + 12, startTime, 0, startTime.Length);
            bool byteSwapFlag = flagByteSwap();
            numSamples = Utility.uBytesToInt(buf[offset + 22], buf[offset + 23], byteSwapFlag);
            sampleRateFactor = Utility.bytesToInt(buf[offset + 24], buf[offset + 25], byteSwapFlag);
            sampleRateMultiplier = Utility.bytesToInt(buf[offset + 26], buf[offset + 27], byteSwapFlag);
            activityFlags = buf[offset + 28];
            ioClockFlags = buf[offset + 29];
            dataQualityFlags = buf[offset + 30];
            numBlockettes = buf[offset + 31];
            timeCorrection = Utility.bytesToInt(buf[offset + 32], buf[offset + 33], buf[offset + 34], buf[offset + 35], byteSwapFlag);
            dataOffset = Utility.uBytesToInt(buf[offset + 36], buf[offset + 37], byteSwapFlag);
            dataBlocketteOffset = Utility.uBytesToInt(buf[offset + 38], buf[offset + 39], byteSwapFlag);
        }

        /// <summary>
        /// write DataHeader contents to a DataOutput stream
        /// </summary>
        /// <param name="dos">
        ///            DataOutput stream to write to </param>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: protected void write(java.io.DataOutput dos) throws java.io.IOException
        protected internal virtual void write(DataOutput dos)
        {
            base.write(dos);
            dos.write(Utility.pad(StationIdentifier.GetBytes(Encoding.ASCII), 5, (sbyte)32));
            dos.write(Utility.pad(LocationIdentifier.GetBytes(Encoding.ASCII), 2, (sbyte)32));
            dos.write(Utility.pad(ChannelIdentifier.GetBytes(Encoding.ASCII), 3, (sbyte)32));
            dos.write(Utility.pad(NetworkCode.GetBytes(Encoding.ASCII), 2, (sbyte)32));
            dos.write(startTime);
            dos.writeShort((short)getNumSamples());
            dos.writeShort((short)getSampleRateFactor());
            dos.writeShort((short)getSampleRateMultiplier());
            dos.writeByte(ActivityFlags);
            dos.writeByte(IOClockFlags);
            dos.writeByte(DataQualityFlags);
            dos.writeByte(NumBlockettes);
            dos.writeInt(TimeCorrection);
            dos.writeShort((short)getDataOffset());
            dos.writeShort((short)getDataBlocketteOffset());
        }

        public virtual short Size
        {
            get
            {
                return 48;
            }
        }

        /// <summary>
        /// same as getTypeCode() in ControlHeader, just a convenience method as the type code is called a
        /// Data header/quality indicator in the seed documentation for data header.
        /// </summary>
        public virtual char QualityIndicator
        {
            get
            {
                return TypeCode;
            }
        }
        /// <summary>
        /// Get the value of stationIdentifier.
        /// </summary>
        /// <returns> Value of stationIdentifier. </returns>
        public virtual string StationIdentifier
        {
            get
            {
                if (stationIdentifierString == null)
                {
                    stationIdentifierString = new string(stationIdentifier);
                }
                return stationIdentifierString;
            }
            set
            {
                stationIdentifierString = value;
                try
                {
                    this.stationIdentifier = Utility.pad(value.GetBytes(Encoding.ASCII), 5, (sbyte)32);
                }
                catch (java.io.UnsupportedEncodingException e)
                {
                    throw new Exception("Shouldn't happen", e);
                }
            }
        }


        /// <summary>
        /// Get the value of locationIdentifier.
        /// </summary>
        /// <returns> Value of locationIdentifier. </returns>
        public virtual string LocationIdentifier
        {
            get
            {
                if (locationIdentifierString == null)
                {
                    locationIdentifierString = new string(locationIdentifier);
                }
                return locationIdentifierString;
            }
            set
            {
                locationIdentifierString = value;
                int requiredBytes = 2; // REFER SEED Format
                try
                {
                    this.locationIdentifier = Utility.pad(value.GetBytes(Encoding.ASCII), requiredBytes, (sbyte)32);
                }
                catch (java.io.UnsupportedEncodingException e)
                {
                    throw new Exception("Shouldn't happen", e);
                }
            }
        }


        /// <summary>
        /// Get the value of channelIdentifier.
        /// </summary>
        /// <returns> Value of channelIdentifier. </returns>
        public virtual string ChannelIdentifier
        {
            get
            {
                if (channelIdentifierString == null)
                {
                    channelIdentifierString = new string(channelIdentifier);
                }
                return channelIdentifierString;
            }
            set
            {
                channelIdentifierString = value;
                int requiredBytes = 3; // REFER SEED Format
                try
                {
                    this.channelIdentifier = Utility.pad(value.GetBytes(Encoding.ASCII), requiredBytes, (sbyte)32);
                }
                catch (java.io.UnsupportedEncodingException e)
                {
                    throw new Exception("Shouldn't happen", e);
                }
            }
        }


        /// <summary>
        /// Get the value of networkCode.
        /// </summary>
        /// <returns> Value of networkCode. </returns>
        public virtual string NetworkCode
        {
            get
            {
                if (networkCodeString == null)
                {
                    networkCodeString = new string(networkCode);
                }
                return networkCodeString;
            }
            set
            {
                networkCodeString = value;
                int requiredBytes = 2; // REFER SEED FORMAT
                sbyte paddingByte = (sbyte)32;
                try
                {
                    this.networkCode = Utility.pad(value.GetBytes(Encoding.ASCII), requiredBytes, paddingByte);
                }
                catch (java.io.UnsupportedEncodingException e)
                {
                    throw new Exception("Shouldn't happen", e);
                }
            }
        }


        // extract SEED time structure from the startTime byte vector, splitting
        // into
        // meaningful elements of year, day, time
        // Btime is an inner class
        public virtual Btime StartBtime
        {
            get
            {
                return new Btime(startTime);
            }
            set
            {
                this.startTime = value.AsBytes;
            }
        }


        /// <summary>
        /// get the sample rate. derived from sample rate factor and the sample rate
        /// multiplier. Note this may not be the true sample rate if the record contains
        /// a blockette 100.
        /// </summary>
        /// <returns> sample rate </returns>
        /// @deprecated Use DataRecord.getSampleRate() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getSampleRate() as it also checks for a possible blockette100 value.")]
        public virtual float getSampleRate()
        {
            return calcSampleRateFromMultipilerFactor();
        }

        // convert contents of Btime structure to the number of
        // ten thousandths of seconds it represents within that year
        private static double ttConvert(Btime bTime)
        {
            double tenThousandths = bTime.jday * 864000000.0;
            tenThousandths += bTime.hour * 36000000.0;
            tenThousandths += bTime.min * 600000.0;
            tenThousandths += bTime.sec * 10000.0;
            tenThousandths += bTime.tenthMilli;
            return tenThousandths;
        }

        // take the Btime structure and forward-project a new time that is
        // the specified number of ten thousandths of seconds ahead
        internal static Btime projectTime(Btime bTime, double tenThousandths)
        {
            int offset = 0; // leap year offset
                            // check to see if this is a leap year we are starting on
            bool is_leap = bTime.year % 4 == 0 && bTime.year % 100 != 0 || bTime.year % 400 == 0;
            if (is_leap)
            {
                offset = 1;
            }
            // convert bTime to tenths of seconds in the current year, then
            // add that value to the incremental time value tenThousandths
            tenThousandths += ttConvert(bTime);
            // now increment year if it crosses the year boundary
            if ((tenThousandths) >= (366 + offset) * 864000000.0)
            {
                bTime.year++;
                tenThousandths -= (365 + offset) * 864000000.0;
            }
            // increment day
            bTime.jday = (int)(tenThousandths / 864000000.0);
            tenThousandths -= (double)bTime.jday * 864000000.0;
            // increment hour
            bTime.hour = (int)(tenThousandths / 36000000.0);
            tenThousandths -= (double)bTime.hour * 36000000.0;
            // increment minutes
            bTime.min = (int)(tenThousandths / 600000.0);
            tenThousandths -= (double)bTime.min * 600000.0;
            // increment seconds
            bTime.sec = (int)(tenThousandths / 10000.0);
            tenThousandths -= (double)bTime.sec * 10000.0;
            // set tenth seconds
            bTime.tenthMilli = (int)tenThousandths;
            // return the resultant value
            return bTime;
        }

        /// <summary>
        /// return a Btime structure containing the derived end time for this record
        /// Note this is not the time of the last sample, but rather the predicted
        /// begin time of the next record, ie begin + numSample*period instead of
        /// begin + (numSample-1)*period.
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100. </summary>
        /// @deprecated Use DataRecord.getEndBtime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getEndBtime() as it also checks for a possible blockette100 value.")]
        private Btime EndBtime
        {
            get
            {
                Btime startBtime = StartBtime;
                // get the number of ten thousandths of seconds of data
                double numTenThousandths = (((double)getNumSamples() / getSampleRate()) * 10000.0);
                // return the time structure projected by the number of ten thousandths
                // of seconds
                return projectTime(startBtime, numTenThousandths);
            }
        }
        /// <summary>
        /// returns the predicted start time of the next record, ie begin + numSample*period
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100.
        /// </summary>
        /// @deprecated Use DataRecord.getPredictedNextStartBtime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getPredictedNextStartBtime() as it also checks for a possible blockette100 value.")]
        public virtual Btime PredictedNextStartBtime
        {
            get
            {
                return EndBtime;
            }
        }

        /// @deprecated Use DataRecord.getBtimeRange() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getBtimeRange() as it also checks for a possible blockette100 value.")]
        public virtual BtimeRange BtimeRange
        {
            get
            {
                return new BtimeRange(StartBtime, LastSampleBtime);
            }
        }

        /// <summary>
        /// return a Btime structure containing the derived last sample time for this
        /// record.
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100. </summary>
        /// @deprecated Use DataRecord.getLastSampleBtime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getLastSampleBtime() as it also checks for a possible blockette100 value.")]
        public virtual Btime LastSampleBtime
        {
            get
            {
                Btime startBtime = StartBtime;
                // get the number of ten thousandths of seconds of data
                double numTenThousandths = (((double)(getNumSamples() - 1) / SampleRate) * 10000.0);
                // return the time structure projected by the number of ten thousandths
                // of seconds
                return projectTime(startBtime, numTenThousandths);
            }
        }

        /// <summary>
        /// Get the value of startTime.
        /// </summary>
        /// <returns> Value of startTime. </returns>
        public virtual string StartTime
        {
            get
            {
                // get time structure
                Btime startStruct = StartBtime;
                // zero padding format of output numbers
                DecimalFormat twoZero = new DecimalFormat("00");
                DecimalFormat threeZero = new DecimalFormat("000");
                DecimalFormat fourZero = new DecimalFormat("0000");
                // return string in standard jday format
                return new string(fourZero.format(startStruct.year) + "," + threeZero.format(startStruct.jday) + "," + twoZero.format(startStruct.hour) + ":" + twoZero.format(startStruct.min) + ":" + twoZero.format(startStruct.sec) + "." + fourZero.format(startStruct.tenthMilli));
            }
        }

        /// <summary>
        /// get the value of end time. derived from Start time, sample rate, and
        /// number of samples. Note this is not the time of the last sample, but
        /// rather the predicted begin time of the next record.
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100.
        /// </summary>
        /// <returns> the value of end time </returns>
        /// @deprecated Use DataRecord.getEndTime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getEndTime() as it also checks for a possible blockette100 value.")]
        public virtual string EndTime
        {
            get
            {
                // get time structure
                Btime endStruct = EndBtime;
                // zero padding format of output numbers
                DecimalFormat twoZero = new DecimalFormat("00");
                DecimalFormat threeZero = new DecimalFormat("000");
                DecimalFormat fourZero = new DecimalFormat("0000");
                // return string in standard jday format
                return new string(fourZero.format(endStruct.year) + "," + threeZero.format(endStruct.jday) + "," + twoZero.format(endStruct.hour) + ":" + twoZero.format(endStruct.min) + ":" + twoZero.format(endStruct.sec) + "." + fourZero.format(endStruct.tenthMilli));
            }
        }

        /// <summary>
        /// get the value of end time. derived from Start time, sample rate, and
        /// number of samples.
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100.
        /// </summary>
        /// <returns> the value of end time </returns>
        /// @deprecated Use DataRecord.getLastSampleTime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getLastSampleTime() as it also checks for a possible blockette100 value.")]
        public virtual string LastSampleTime
        {
            get
            {
                // get time structure
                Btime endStruct = LastSampleBtime;
                // zero padding format of output numbers
                DecimalFormat twoZero = new DecimalFormat("00");
                DecimalFormat threeZero = new DecimalFormat("000");
                DecimalFormat fourZero = new DecimalFormat("0000");
                // return string in standard jday format
                return new string(fourZero.format(endStruct.year) + "," + threeZero.format(endStruct.jday) + "," + twoZero.format(endStruct.hour) + ":" + twoZero.format(endStruct.min) + ":" + twoZero.format(endStruct.sec) + "." + fourZero.format(endStruct.tenthMilli));
            }
        }

        /// <summary>
        /// Get the value of numSamples.
        /// </summary>
        /// <returns> Value of numSamples. </returns>
        public virtual int getNumSamples()
        {
            return numSamples;
        }

        /// <summary>
        /// Set the value of numSamples.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to numSamples. </param>
        public virtual void setNumSamples(short v)
        {
            this.numSamples = v;
        }

        /// <summary>
        /// Get the value of sampleRateFactor.
        /// </summary>
        /// <returns> Value of sampleRateFactor. </returns>
        public virtual int getSampleRateFactor()
        {
            return sampleRateFactor;
        }

        /// <summary>
        /// Set the value of sampleRateFactor.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to sampleRateFactor. </param>
        public virtual void setSampleRateFactor(short v)
        {
            this.sampleRateFactor = v;
        }

        /// <summary>
        /// Get the value of sampleRateMultiplier.
        /// </summary>
        /// <returns> Value of sampleRateMultiplier. </returns>
        public virtual int getSampleRateMultiplier()
        {
            return sampleRateMultiplier;
        }

        /// <summary>
        /// Set the value of sampleRateMultiplier.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to sampleRateMultiplier. </param>
        public virtual void setSampleRateMultiplier(short v)
        {
            this.sampleRateMultiplier = v;
        }

        public virtual double SampleRate
        {
            set
            {
                short[] tmp = calcSeedMultipilerFactor(value);
                setSampleRateFactor(tmp[0]);
                setSampleRateMultiplier(tmp[1]);
            }
        }

        /// <summary>
        /// get the sample rate. derived from sample rate factor and the sample rate
        /// multiplier. Note this may not be the true sample rate if the record contains
        /// a blockette 100.
        /// 
        /// Returns zero if either of the multiplier or factor are zero, usually in the case of log/ascii/opaque data. </summary>
        /// <returns> sample rate </returns>
        public virtual float calcSampleRateFromMultipilerFactor()
        {
            double factor = (double)getSampleRateFactor();
            double multiplier = (double)getSampleRateMultiplier();
            float sampleRate;
            if ((factor * multiplier) != 0.0)
            { // in the case of log records
                sampleRate = (float)(Math.Pow(Math.Abs(factor), (factor / Math.Abs(factor))) * Math.Pow(Math.Abs(multiplier), (multiplier / Math.Abs(multiplier))));
            }
            else
            {
                // log/ascii/opaque data
                sampleRate = 0;
            }
            return sampleRate;
        }

        public static short[] calcSeedMultipilerFactor(double sps)
        {
            if (sps >= 1)
            {
                // don't get too close to the max for a short, use ceil as neg
                int divisor = (int)Math.Ceiling((short.MinValue + 2) / sps);
                // don't get too close to the max for a short
                if (divisor < short.MinValue + 2)
                {
                    divisor = short.MinValue + 2;
                }
                int factor = (int)(long)Math.Round(-1 * sps * divisor, MidpointRounding.AwayFromZero);
                return new short[] { (short)factor, (short)divisor };
            }
            else
            {
                // don't get too close to the max for a short, use ceil as neg
                int factor = -1 * (int)(long)Math.Round(Math.Floor(1.0 * sps * (short.MaxValue - 2)) / sps, MidpointRounding.AwayFromZero);
                // don't get too close to the max for a short
                if (factor > short.MaxValue - 2)
                {
                    factor = short.MaxValue - 2;
                }
                int divisor = (int)(long)Math.Round(-1 * factor * sps, MidpointRounding.AwayFromZero);
                return new short[] { (short)factor, (short)divisor };
            }
        }

        /// <summary>
        /// Get the value of activityFlags.
        /// </summary>
        /// <returns> Value of activityFlags. </returns>
        public virtual sbyte ActivityFlags
        {
            get
            {
                return activityFlags;
            }
            set
            {
                this.activityFlags = value;
            }
        }


        /// <summary>
        /// Get the value of IOClockFlags.
        /// </summary>
        /// <returns> Value of IOClockFlags. </returns>
        public virtual sbyte IOClockFlags
        {
            get
            {
                return ioClockFlags;
            }
            set
            {
                this.ioClockFlags = value;
            }
        }

        /// <summary>
        /// returns the predicted start time of the next record, ie begin + numSample*period
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100.
        /// </summary>
        /// @deprecated Use DataRecord.getPredictedNextStartBtime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getPredictedNextStartBtime() as it also checks for a possible blockette100 value.")]
        public virtual Btime PredictedNextStartBtime
        {
            get
            {
                return EndBtime;
            }
        }

        /// @deprecated Use DataRecord.getBtimeRange() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getBtimeRange() as it also checks for a possible blockette100 value.")]
        public virtual BtimeRange BtimeRange
        {
            get
            {
                return new BtimeRange(StartBtime, LastSampleBtime);
            }
        }

        /// <summary>
        /// return a Btime structure containing the derived last sample time for this
        /// record.
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100. </summary>
        /// @deprecated Use DataRecord.getLastSampleBtime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getLastSampleBtime() as it also checks for a possible blockette100 value.")]
        public virtual Btime LastSampleBtime
        {
            get
            {
                Btime startBtime = StartBtime;
                // get the number of ten thousandths of seconds of data
                double numTenThousandths = (((double)(getNumSamples() - 1) / SampleRate) * 10000.0);
                // return the time structure projected by the number of ten thousandths
                // of seconds
                return projectTime(startBtime, numTenThousandths);
            }
        }

        /// <summary>
        /// Get the value of startTime.
        /// </summary>
        /// <returns> Value of startTime. </returns>
        public virtual string StartTime
        {
            get
            {
                // get time structure
                Btime startStruct = StartBtime;
                // zero padding format of output numbers
                DecimalFormat twoZero = new DecimalFormat("00");
                DecimalFormat threeZero = new DecimalFormat("000");
                DecimalFormat fourZero = new DecimalFormat("0000");
                // return string in standard jday format
                return new string(fourZero.format(startStruct.year) + "," + threeZero.format(startStruct.jday) + "," + twoZero.format(startStruct.hour) + ":" + twoZero.format(startStruct.min) + ":" + twoZero.format(startStruct.sec) + "." + fourZero.format(startStruct.tenthMilli));
            }
        }

        /// <summary>
        /// get the value of end time. derived from Start time, sample rate, and
        /// number of samples. Note this is not the time of the last sample, but
        /// rather the predicted begin time of the next record.
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100.
        /// </summary>
        /// <returns> the value of end time </returns>
        /// @deprecated Use DataRecord.getEndTime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getEndTime() as it also checks for a possible blockette100 value.")]
        public virtual string EndTime
        {
            get
            {
                // get time structure
                Btime endStruct = EndBtime;
                // zero padding format of output numbers
                DecimalFormat twoZero = new DecimalFormat("00");
                DecimalFormat threeZero = new DecimalFormat("000");
                DecimalFormat fourZero = new DecimalFormat("0000");
                // return string in standard jday format
                return new string(fourZero.format(endStruct.year) + "," + threeZero.format(endStruct.jday) + "," + twoZero.format(endStruct.hour) + ":" + twoZero.format(endStruct.min) + ":" + twoZero.format(endStruct.sec) + "." + fourZero.format(endStruct.tenthMilli));
            }
        }

        /// <summary>
        /// get the value of end time. derived from Start time, sample rate, and
        /// number of samples.
        /// 
        /// Note that this may not be correct if the record also contains a more accurate
        /// sample rate in a blockette100.
        /// </summary>
        /// <returns> the value of end time </returns>
        /// @deprecated Use DataRecord.getLastSampleTime() as it also checks for a possible blockette100 value.  
        [Obsolete("Use DataRecord.getLastSampleTime() as it also checks for a possible blockette100 value.")]
        public virtual string LastSampleTime
        {
            get
            {
                // get time structure
                Btime endStruct = LastSampleBtime;
                // zero padding format of output numbers
                DecimalFormat twoZero = new DecimalFormat("00");
                DecimalFormat threeZero = new DecimalFormat("000");
                DecimalFormat fourZero = new DecimalFormat("0000");
                // return string in standard jday format
                return new string(fourZero.format(endStruct.year) + "," + threeZero.format(endStruct.jday) + "," + twoZero.format(endStruct.hour) + ":" + twoZero.format(endStruct.min) + ":" + twoZero.format(endStruct.sec) + "." + fourZero.format(endStruct.tenthMilli));
            }
        }

        /// <summary>
        /// Get the value of numSamples.
        /// </summary>
        /// <returns> Value of numSamples. </returns>
        public virtual int getNumSamples()
        {
            return numSamples;
        }

        /// <summary>
        /// Set the value of numSamples.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to numSamples. </param>
        public virtual void setNumSamples(short v)
        {
            this.numSamples = v;
        }

        /// <summary>
        /// Get the value of sampleRateFactor.
        /// </summary>
        /// <returns> Value of sampleRateFactor. </returns>
        public virtual int getSampleRateFactor()
        {
            return sampleRateFactor;
        }

        /// <summary>
        /// Set the value of sampleRateFactor.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to sampleRateFactor. </param>
        public virtual void setSampleRateFactor(short v)
        {
            this.sampleRateFactor = v;
        }

        /// <summary>
        /// Get the value of sampleRateMultiplier.
        /// </summary>
        /// <returns> Value of sampleRateMultiplier. </returns>
        public virtual int getSampleRateMultiplier()
        {
            return sampleRateMultiplier;
        }

        /// <summary>
        /// Set the value of sampleRateMultiplier.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to sampleRateMultiplier. </param>
        public virtual void setSampleRateMultiplier(short v)
        {
            this.sampleRateMultiplier = v;
        }

        public virtual double SampleRate
        {
            set
            {
                short[] tmp = calcSeedMultipilerFactor(value);
                setSampleRateFactor(tmp[0]);
                setSampleRateMultiplier(tmp[1]);
            }
        }

        /// <summary>
        /// get the sample rate. derived from sample rate factor and the sample rate
        /// multiplier. Note this may not be the true sample rate if the record contains
        /// a blockette 100.
        /// 
        /// Returns zero if either of the multiplier or factor are zero, usually in the case of log/ascii/opaque data. </summary>
        /// <returns> sample rate </returns>
        public virtual float calcSampleRateFromMultipilerFactor()
        {
            double factor = (double)getSampleRateFactor();
            double multiplier = (double)getSampleRateMultiplier();
            float sampleRate;
            if ((factor * multiplier) != 0.0)
            { // in the case of log records
                sampleRate = (float)(Math.Pow(Math.Abs(factor), (factor / Math.Abs(factor))) * Math.Pow(Math.Abs(multiplier), (multiplier / Math.Abs(multiplier))));
            }
            else
            {
                // log/ascii/opaque data
                sampleRate = 0;
            }
            return sampleRate;
        }

        public static short[] calcSeedMultipilerFactor(double sps)
        {
            if (sps >= 1)
            {
                // don't get too close to the max for a short, use ceil as neg
                int divisor = (int)Math.Ceiling((short.MinValue + 2) / sps);
                // don't get too close to the max for a short
                if (divisor < short.MinValue + 2)
                {
                    divisor = short.MinValue + 2;
                }
                int factor = (int)(long)Math.Round(-1 * sps * divisor, MidpointRounding.AwayFromZero);
                return new short[] { (short)factor, (short)divisor };
            }
            else
            {
                // don't get too close to the max for a short, use ceil as neg
                int factor = -1 * (int)(long)Math.Round(Math.Floor(1.0 * sps * (short.MaxValue - 2)) / sps, MidpointRounding.AwayFromZero);
                // don't get too close to the max for a short
                if (factor > short.MaxValue - 2)
                {
                    factor = short.MaxValue - 2;
                }
                int divisor = (int)(long)Math.Round(-1 * factor * sps, MidpointRounding.AwayFromZero);
                return new short[] { (short)factor, (short)divisor };
            }
        }
        /// <summary>
        /// Get the value of activityFlags.
        /// 
        /// 
        /// 
        /// /**
        /// Get the value of dataQualityFlags.
        /// </summary>
        /// <returns> Value of dataQualityFlags. </returns>
        public virtual sbyte DataQualityFlags
        {
            get
            {
                return dataQualityFlags;
            }
            set
            {
                this.dataQualityFlags = value;
            }
        }


        /// <summary>
        /// Get the value of numBlockettes.
        /// </summary>
        /// <returns> Value of numBlockettes. </returns>
        public virtual sbyte NumBlockettes
        {
            get
            {
                return numBlockettes;
            }
            set
            {
                this.numBlockettes = value;
            }
        }


        /// <summary>
        /// Get the value of timeCorrection.
        /// </summary>
        /// <returns> Value of timeCorrection. </returns>
        public virtual int TimeCorrection
        {
            get
            {
                return timeCorrection;
            }
            set
            {
                this.timeCorrection = value;
            }
        }


        /// <summary>
        /// Get the value of dataOffset.
        /// </summary>
        /// <returns> Value of dataOffset. </returns>
        public virtual int getDataOffset()
        {
            return dataOffset;
        }

        /// <summary>
        /// Set the value of dataOffset.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to dataOffset. </param>
        public virtual void setDataOffset(short v)
        {
            this.dataOffset = v;
        }

        /// <summary>
        /// Get the value of dataBlocketteOffset.
        /// </summary>
        /// <returns> Value of dataBlocketteOffset. </returns>
        public virtual int getDataBlocketteOffset()
        {
            return dataBlocketteOffset;
        }

        /// <summary>
        /// Set the value of dataBlocketteOffset.
        /// </summary>
        /// <param name="v">
        ///            Value to assign to dataBlocketteOffset. </param>
        public virtual void setDataBlocketteOffset(short v)
        {
            this.dataBlocketteOffset = v;
        }

        /// <summary>
        /// Present a default string representation of the contents of this object
        /// </summary>
        /// <returns> formatted string of object contents </returns>
        public override string ToString()
        {
            string s = base.ToString() + " ";
            s += " " + NetworkCode + "." + StationIdentifier + "." + LocationIdentifier + "." + ChannelIdentifier + "." + StartTime + "  " + SampleRate * NumSamples + " " + NumBlockettes + " " + getDataOffset() + " " + getDataBlocketteOffset();
            return s;
        }
    }

}
