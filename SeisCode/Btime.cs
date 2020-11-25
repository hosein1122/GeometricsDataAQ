using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeisCode
{
    public class Btime
    {
        public DateTime DateTime { get; private set; }

        public int Year { get { return DateTime.Year; } }
        public int Jday { get { return DateTime.DayOfYear; } }

        public int Hour { get { return DateTime.Hour; } }

        public int Min { get { return DateTime.Minute; } }

        public int Sec { get { return DateTime.Second; } }

        public int TenthMilli { get { return DateTime.Millisecond * 10; } }

        public Btime(DateTime datetime)
        {
            DateTime = datetime;
        }

        public Btime(byte[] bytes, int offset)
        {
            bool byteSwapFlag = ShouldSwapBytes(bytes, offset);
            var year = Utility.uBytesToInt(bytes[offset],
                                       bytes[offset + 1],
                                       byteSwapFlag);
            var jday = Utility.uBytesToInt(bytes[offset + 2],
                                       bytes[offset + 3],
                                       byteSwapFlag);
            var hour = bytes[offset + 4] & 0xff;
            var min = bytes[offset + 5] & 0xff;
            var sec = bytes[offset + 6] & 0xff;
            // bytes[7] is unused (alignment)
            var tenthMilli = Utility.uBytesToInt(bytes[offset + 8],
                                              bytes[offset + 9],
                                              byteSwapFlag);
            var day = 1;
            var month = 1;
            DateTime = new DateTime(year, month, day, hour, min, sec, tenthMilli / 10, DateTimeKind.Utc).AddDays(jday - 1);
            //new DateTime(getYear(), getMonth(), getDay(), getHour(), getMin(), getSec(), getTenthMilli() / 10, DateTimeKind.Utc);//.AddDays(getJDay()-1);
        }

        public Btime(int year, int jday, int hour, int min, int sec, int tenthMilli)
        {
            DateTime = new DateTime(year, 1, 1, hour, min, sec, tenthMilli / 10, DateTimeKind.Utc).AddDays(jday - 1);
        }
        public override bool Equals(Object o)
        {
            if (o == this)
            {
                return true;
            }
            if (o is Btime)
            {
                Btime oBtime = (Btime)o;
                return oBtime.DateTime == DateTime;
            }
            return false;
        }

        public bool Before(Btime other)
        {
            if (other.DateTime > DateTime)
                return true;
            return false;


        }

        public bool After(Btime other)
        {
            if (other.DateTime < DateTime)
                return true;
            return false;

        }

        public bool AfterOrEquals(Btime other)
        {
            if (other.DateTime <= DateTime)
                return true;
            return false;

        }


        /**
         * Expects btime to be a byte array pointing at the beginning of a btime
         * segment
         * 
         * @return - true if the bytes need to be swapped to get a valid year
         */
        public static bool ShouldSwapBytes(byte[] btime)
        {
            return ShouldSwapBytes(btime, 0);
        }

        /**
         * Expects btime to be a byte array pointing at the beginning of a btime
         * segment.
         * 
         * Time capsule: note that year 2056 as a short byte swaps to itself, so whomever
         * is maintaining this code off in the distant future, 49 years from now as
         * I write this in 2007, should find some other header to use for byte swap checking!
         * 
         * Using the jday or tenthmilli doesn't help much as 1 byte swaps to 256, 256 to 1 and 257 to itself.
         * 
         * If mseed was going to support little endian headers they should have put in a damn flag!
         *  - HPC
         * 
         * @return - true if the bytes need to be swapped to get a valid year
         */
        public static bool ShouldSwapBytes(byte[] btime, int offset)
        {
            int year = Utility.uBytesToInt(btime[0 + offset],
                                           btime[1 + offset],
                                           false);
            return year < 1960 || year > 2055;
        }



        public byte[] Bytes
        {
            get
            {
                byte[] bytes = new byte[10];
                Array.Copy(Utility.intToByteArray(DateTime.Year), 2, bytes, 0, 2);
                Array.Copy(Utility.intToByteArray(DateTime.DayOfYear), 2, bytes, 2, 2);
                Array.Copy(Utility.intToByteArray(DateTime.Hour), 3, bytes, 4, 1);
                Array.Copy(Utility.intToByteArray(DateTime.Minute), 3, bytes, 5, 1);
                Array.Copy(Utility.intToByteArray(DateTime.Second), 3, bytes, 6, 1);
                Array.Copy(Utility.intToByteArray(DateTime.Millisecond * 10), 2, bytes, 8, 2);
                return bytes;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
