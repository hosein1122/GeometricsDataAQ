using System;
using System.Collections.Generic;
using System.IO;
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

        protected internal byte[] stationIdentifier = new byte[5];

        protected internal string stationIdentifierString;

        protected internal byte[] locationIdentifier = new byte[2];

        protected internal string locationIdentifierString;

        protected internal byte[] channelIdentifier = new byte[3];

        protected internal string channelIdentifierString;

        protected internal byte[] networkCode = new byte[2];

        protected internal string networkCodeString;

        protected internal byte[] startTime = new byte[10];

        protected internal int numSamples;

        protected internal int sampleRateFactor;

        protected internal int sampleRateMultiplier;

        protected internal byte activityFlags;

        protected internal byte ioClockFlags;

        protected internal byte dataQualityFlags;

        protected internal byte numBlockettes;

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
        public override void WriteASCII(TextWriter @out)
        {
            WriteASCII(@out, "");
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void writeASCII(java.io.PrintWriter out, String indent) throws java.io.IOException
        public override void WriteASCII(TextWriter @out, string indent)
        {
            base.WriteASCII(@out, indent);
            @out.Write(indent + NetworkCode.Trim() + "." + StationIdentifier.Trim() + "." + LocationIdentifier + "." + ChannelIdentifier);
            @out.Write(" start=" + StartTime);
            @out.Write(" numPTS=" + NumSamples);
            @out.Write(" sampFac=" + SampleRateFactor);
            @out.Write(" sampMul=" + SampleRateMultiplier);
            @out.Write(" ac=" + ActivityFlags);
            @out.Write(" io=" + IOClockFlags);
            @out.Write(" qual=" + DataQualityFlags);
            @out.Write(" numBlockettes=" + NumBlockettes);
            @out.Write(" blocketteOffset=" + DataBlocketteOffset);
            @out.Write(" dataOffset=" + DataOffset);
            @out.WriteLine(" tcor=" + TimeCorrection);
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
        public static DataHeader read(BinaryReader @in, int sequenceNum, char typeCode, bool continuationCode)
        {
            byte[] buf = @in.ReadBytes(40);
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
            return Btime.ShouldSwapBytes(startTime);
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
        protected internal virtual void read(byte[] buf, int offset)
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
        protected internal virtual void write(BinaryWriter dos)
        {
            base.Write(dos);
            dos.Write(Utility.pad(Encoding.ASCII.GetBytes(StationIdentifier), 5, (byte)32));
            dos.Write(Utility.pad(Encoding.ASCII.GetBytes(LocationIdentifier), 2, (byte)32));
            dos.Write(Utility.pad(Encoding.ASCII.GetBytes(ChannelIdentifier), 3, (byte)32));
            dos.Write(Utility.pad(Encoding.ASCII.GetBytes(NetworkCode), 2, (byte)32));
            dos.Write(startTime);
            dos.Write((short)NumSamples);
            dos.Write((short)SampleRateFactor);
            dos.Write((short)SampleRateMultiplier);
            dos.Write(ActivityFlags);
            dos.Write(IOClockFlags);
            dos.Write(DataQualityFlags);
            dos.Write(NumBlockettes);
            dos.Write(TimeCorrection);
            dos.Write((short)DataOffset);
            dos.Write((short)DataBlocketteOffset);
        }

        public override short Size
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
                    stationIdentifierString = Encoding.ASCII.GetString(stationIdentifier);
                }
                return stationIdentifierString;
            }
            set
            {
                stationIdentifierString = value;
                try
                {
                    this.stationIdentifier = Utility.pad(Encoding.ASCII.GetBytes(value), 5, (byte)32);
                }
                catch (Exception e)
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
                    locationIdentifierString = Encoding.ASCII.GetString(locationIdentifier);
                }
                return locationIdentifierString;
            }
            set
            {
                locationIdentifierString = value;
                int requiredBytes = 2; // REFER SEED Format
                try
                {
                    this.locationIdentifier = Utility.pad(Encoding.ASCII.GetBytes(value), requiredBytes, (byte)32);
                }
                catch (Exception e)
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
                    channelIdentifierString = Encoding.ASCII.GetString(channelIdentifier);
                }
                return channelIdentifierString;
            }
            set
            {
                channelIdentifierString = value;
                int requiredBytes = 3; // REFER SEED Format
                try
                {
                    this.channelIdentifier = Utility.pad(Encoding.ASCII.GetBytes(value), requiredBytes, (byte)32);
                }
                catch (Exception e)
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
                    networkCodeString = Encoding.ASCII.GetString(networkCode);
                }
                return networkCodeString;
            }
            set
            {
                networkCodeString = value;
                int requiredBytes = 2; // REFER SEED FORMAT
                byte paddingByte = (byte)32;
                try
                {
                    this.networkCode = Utility.pad(Encoding.ASCII.GetBytes(value), requiredBytes, paddingByte);
                }
                catch (Exception e)
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
                return new Btime(startTime, 0);
            }
            set
            {
                startTime = value.Bytes;
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
            return CalcSampleRateFromMultipilerFactor();
        }

        // convert contents of Btime structure to the number of
        // ten thousandths of seconds it represents within that year
        private static double ttConvert(Btime bTime)
        {
            double tenThousandths = bTime.Jday * 864000000.0;
            tenThousandths += bTime.Hour * 36000000.0;
            tenThousandths += bTime.Min * 600000.0;
            tenThousandths += bTime.Sec * 10000.0;
            tenThousandths += bTime.TenthMilli;
            return tenThousandths;
        }

        // take the Btime structure and forward-project a new time that is
        // the specified number of ten thousandths of seconds ahead
        public  static Btime ProjectTime(Btime bTime, double tenThousandths)
        {
            //int offset = 0; // leap year offset
            //                // check to see if this is a leap year we are starting on
            //bool is_leap = bTime.Year % 4 == 0 && bTime.Year % 100 != 0 || bTime.Year % 400 == 0;
            //if (is_leap)
            //{
            //    offset = 1;
            //}
            //// convert bTime to tenths of seconds in the current year, then
            //// add that value to the incremental time value tenThousandths
            //tenThousandths += ttConvert(bTime);
            //// now increment year if it crosses the year boundary
            //if ((tenThousandths) >= (366 + offset) * 864000000.0)
            //{
            //    bTime.year++;
            //    tenThousandths -= (365 + offset) * 864000000.0;
            //}
            //// increment day
            //bTime.jday = (int)(tenThousandths / 864000000.0);
            //tenThousandths -= (double)bTime.jday * 864000000.0;
            //// increment hour
            //bTime.hour = (int)(tenThousandths / 36000000.0);
            //tenThousandths -= (double)bTime.hour * 36000000.0;
            //// increment minutes
            //bTime.min = (int)(tenThousandths / 600000.0);
            //tenThousandths -= (double)bTime.min * 600000.0;
            //// increment seconds
            //bTime.sec = (int)(tenThousandths / 10000.0);
            //tenThousandths -= (double)bTime.sec * 10000.0;
            //// set tenth seconds
            //bTime.tenthMilli = (int)tenThousandths;
            //// return the resultant value
            //return bTime;
            int offset = 0; // leap year offset
            // check to see if this is a leap year we are starting on
            bool is_leap = bTime.Year % 4 == 0 && bTime.Year % 100 != 0
                    || bTime.Year % 400 == 0;
            if (is_leap)
                offset = 1;
            // convert bTime to tenths of seconds in the current year, then
            // add that value to the incremental time value tenThousandths
            tenThousandths += ttConvert(bTime);
            // now increment year if it crosses the year boundary

            var year = bTime.Year;
            if ((tenThousandths) >= (366 + offset) * 864000000.0)
            {
                year++;
                tenThousandths -= (365 + offset) * 864000000.0;
            }
            // increment day
            var jday = (int)(tenThousandths / 864000000.0);
            tenThousandths -= (double)jday * 864000000.0;
            // increment hour
            var hour = (int)(tenThousandths / 36000000.0);
            tenThousandths -= (double)hour * 36000000.0;
            // increment minutes
            var min = (int)(tenThousandths / 600000.0);
            tenThousandths -= (double)min * 600000.0;
            // increment seconds
            var sec = (int)(tenThousandths / 10000.0);
            tenThousandths -= (double)sec * 10000.0;
            // set tenth seconds
            var tenthMilli = (int)tenThousandths;
            // return the resultant value
            return new Btime(year, jday, hour, min, sec, tenthMilli);

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
                double numTenThousandths = (((double)NumSamples / getSampleRate()) * 10000.0);
                // return the time structure projected by the number of ten thousandths
                // of seconds
                return ProjectTime(startBtime, numTenThousandths);
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
                double numTenThousandths = (((double)(NumSamples - 1) / SampleRate) * 10000.0);
                // return the time structure projected by the number of ten thousandths
                // of seconds
                return ProjectTime(startBtime, numTenThousandths);
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
                return startStruct.Year.ToString("####") + ","
                   + startStruct.Jday.ToString("###") + ","
                   + startStruct.Hour.ToString("##") + ":"
                   + startStruct.Min.ToString("##") + ":"
                   + startStruct.Sec.ToString("##") + "."
                   + startStruct.TenthMilli.ToString("###");

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
                return endStruct.Year.ToString("####") + ","
                    + endStruct.Jday.ToString("###") + ","
                    + endStruct.Hour.ToString("###") + ":"
                    + endStruct.Min.ToString("###") + ":"
                    + endStruct.Sec.ToString("###") + "."
                    + endStruct.TenthMilli.ToString("####");
            }
        }


        

        /// <summary>
        /// Get the value of sampleRateFactor.
        /// </summary>
        /// <returns> Value of sampleRateFactor. </returns>
        public virtual int SampleRateFactor
        {
            get
            {
                return sampleRateFactor;
            }
            set
            {
                sampleRateFactor = value;
            }
        }



        /// <summary>
        /// Get the value of sampleRateMultiplier.
        /// </summary>
        /// <returns> Value of sampleRateMultiplier. </returns>
        public virtual int SampleRateMultiplier
        {
            get
            {
                return sampleRateMultiplier;
            }
            set
            {
                sampleRateMultiplier = value;
            }
        }


        public virtual double SampleRate
        {
            set
            {
                short[] tmp = CalcSeedMultipilerFactor(value);
                SampleRateFactor = tmp[0];
                SampleRateMultiplier = tmp[1];
            }
            get
            {
                return CalcSampleRateFromMultipilerFactor();
            }
        }

        /// <summary>
        /// get the sample rate. derived from sample rate factor and the sample rate
        /// multiplier. Note this may not be the true sample rate if the record contains
        /// a blockette 100.
        /// 
        /// Returns zero if either of the multiplier or factor are zero, usually in the case of log/ascii/opaque data. </summary>
        /// <returns> sample rate </returns>
        public virtual float CalcSampleRateFromMultipilerFactor()
        {
            double factor = (double)SampleRateFactor;
            double multiplier = (double)SampleRateMultiplier;
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

        public static short[] CalcSeedMultipilerFactor(double sps)
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
        public virtual byte ActivityFlags
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
        public virtual byte IOClockFlags
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
                return endStruct.Year.ToString("####") + ","
                    + endStruct.Jday.ToString("###") + ","
                    + endStruct.Hour.ToString("###") + ":"
                    + endStruct.Min.ToString("###") + ":"
                    + endStruct.Sec.ToString("###") + "."
                    + endStruct.TenthMilli.ToString("####");
            }
        }



        /// <summary>
        /// Get the value of numSamples.
        /// </summary>
        /// <returns> Value of numSamples. </returns>
        public virtual int NumSamples
        {
            get
            {
                return numSamples;
            }
            set
            {
                numSamples = value;
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
        public virtual byte DataQualityFlags
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
        public virtual byte NumBlockettes
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


        public virtual int DataOffset
        {
            get
            {
                return dataOffset;
            }
            set
            {
                dataOffset = value;
            }
        }



        /// <summary>
        /// Get And Set the value of dataBlocketteOffset.
        /// </summary>
        /// <returns> 
        /// Value of dataBlocketteOffset. </returns>

        public virtual int DataBlocketteOffset
        {
            get
            {
                return dataBlocketteOffset;
            }
            set
            {
                dataBlocketteOffset = value;
            }
        }



        /// <summary>
        /// Present a default string representation of the contents of this object
        /// </summary>
        /// <returns> formatted string of object contents </returns>
        public override string ToString()
        {
            string s = base.ToString() + " ";
            s += " " + NetworkCode + "." + StationIdentifier + "." + LocationIdentifier + "." + ChannelIdentifier + "." + StartTime + "  " + SampleRate * NumSamples + " " + NumBlockettes + " " + DataOffset + " " + DataBlocketteOffset;
            return s;
        }
    }

}
