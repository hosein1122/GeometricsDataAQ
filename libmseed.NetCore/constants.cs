using System;
using System.Collections.Generic;
using System.Text;

namespace libmseedNetCore
{
    public class Constants
    {

       
        /* SEED data encoding types */
        public const int MINRECLEN = 128; // Minimum Mini-SEED record length, 2^7 bytes
        public const int MAXRECLEN = 1048576; // Maximum Mini-SEED record length, 2^20 bytes
        public const int DE_ASCII = 0;
        public const int DE_INT16 = 1;
        public const int DE_INT32 = 3;
        public const int DE_FLOAT32 = 4;
        public const int DE_FLOAT64 = 5;
        public const int DE_STEIM1 = 10;
        public const int DE_STEIM2 = 11;
        public const int DE_GEOSCOPE24 = 12;
        public const int DE_GEOSCOPE163 = 13;
        public const int DE_GEOSCOPE164 = 14;
        public const int DE_CDSN = 16;
        public const int DE_SRO = 30;
        public const int DE_DWWSSN = 32;

        /* Library return and error code values, error values should always be negative */
        public const int MS_ENDOFFILE = 1; // End of file reached return value
        public const int MS_NOERROR = 0; // No error
        public const int MS_GENERROR = -1; // Generic unspecified error
        public const int MS_NOTSEED = -2; // Data not SEED
        public const int MS_WRONGLENGTH = -3; // Length of data read was not correct
        public const int MS_OUTOFRANGE = -4; // SEED record length out of range
        public const int MS_UNKNOWNFORMAT = -5; // Unknown data encoding format
        public const int MS_STBADCOMPFLAG = -6; // Steim, invalid compression flag(s)


        /* Error code for routines that normally return a high precision time.
         * The time value corresponds to '1902/1/1 00:00:00.000000' with the
         * default HPTMODULUS */
        public const long HPTERROR = -2145916800000000;


        /* Define the high precision time tick interval as 1/modulus seconds */
        /* Default modulus of 1000000 defines tick interval as a microsecond */
        //#define HPTMODULUS 1000000
        public const long HPTMODULUS = 1000000;


        /* Require a large (>= 64-bit) integer type for hptime_t */
        /// <summary>
        /// A wrapper around <see cref="System.Int64"/>.
        /// </summary>
        public struct hptime_t
        {
            // A struct's fields should not be exposed
            private Int64 value;

            // As we are using implicit conversions we can keep the constructor private
            private hptime_t(Int64 value)
            {
                this.value = value;
            }

            /// <summary>
            /// Implicitly converts a <see cref="System.Int64"/> to a hptime_t.
            /// </summary>
            /// <param name="value">The <see cref="System.Int64"/> to convert.</param>
            /// <returns>A new Record with the specified value.</returns>
            public static implicit operator hptime_t(Int64 value)
            {
                return new hptime_t(value);
            }
            /// <summary>
            /// Implicitly converts a hptime_t to a <see cref="System.Int64"/>.
            /// </summary>
            /// <param name="hptime_t">The hptime_t to convert.</param>
            /// <returns>
            /// A <see cref="System.Int64"/> that is the specified hptime_t's value.
            /// </returns>
            public static implicit operator Int64(hptime_t hptime_t)
            {
                return hptime_t.value;
            }

            
        }

        public struct tm
        {
            public int tm_sec;
            public int tm_min;
            public int tm_hour;
            public int tm_mday;
            public int tm_mon;
            public int tm_year;
            public int tm_wday;
            public int tm_yday;
            public int tm_isdst;
            public bool isNull()
            {
                if (tm_year == 0)
                    return true;
                else
                    return false;
                    

            }
            
        };

        /* SEED binary time */
        public class BTime
        {
            public ushort year;
            public ushort day;
            public ushort hour;
            public ushort min;
            public ushort sec;
            public ushort unused;
            public ushort fract;
        }




        /* Macros to scale between Unix/POSIX epoch time & high precision time */
        //#define MS_EPOCH2HPTIME(X) X * (hptime_t) HPTMODULUS
        public static hptime_t MS_EPOCH2HPTIME(hptime_t X) { return X * (hptime_t)HPTMODULUS; }

        //#define MS_HPTIME2EPOCH(X) X / HPTMODULUS
        public static hptime_t MS_HPTIME2EPOCH(hptime_t X) { return X / (hptime_t)HPTMODULUS; }



        /* Fixed section data of header */

        public class fsdh_s
        {
            public string sequence_number = new string(new char[6]);
            public char dataquality;
            public char reserved;
            public string station = new string(new char[5]);
            public string location = new string(new char[2]);
            public string channel = new string(new char[3]);
            public string network = new string(new char[2]);
            public BTime start_time = new BTime();
            public ushort numsamples;
            public short samprate_fact;
            public short samprate_mult;
            public byte act_flags;
            public byte io_flags;
            public byte dq_flags;
            public byte numblockettes;
            public int time_correct;
            public ushort data_offset;
            public ushort blockette_offset;
        }

        public static class GlobalMembers
        {
            /*
            public static fsdh_s LMP_PACKED = new fsdh_s();
            public static blkt_100_s LMP_PACKED = new blkt_100_s();
            public static blkt_1000_s LMP_PACKED = new blkt_1000_s();
            public static blkt_1001_s LMP_PACKED = new blkt_1001_s();
            */

        }


        


        public class MSRecord_s
        {
            public string record; // Mini-SEED record
            public int reclen; // Length of Mini-SEED record in bytes

            /* Pointers to SEED data record structures */
            public fsdh_s fsdh; // Fixed Section of Data Header

            public BlktLink blkts; // Root of blockette chain
                   


            public blkt_100_s Blkt100; // Blockette 100, if present
            public blkt_1000_s Blkt1000; // Blockette 1000, if present
            public blkt_1001_s Blkt1001; // Blockette 1001, if present

            /* Common header fields in accessible form */
            public int sequence_number; // SEED record sequence number
            public string network = new string(new char[11]); // Network designation, NULL terminated
            public string station = new string(new char[11]); // Station designation, NULL terminated
            public string location = new string(new char[11]); // Location designation, NULL terminated
            public string channel = new string(new char[11]); // Channel designation, NULL terminated
            public char dataquality; // Data quality indicator
            public hptime_t starttime = new hptime_t(); // Record start time, corrected (first sample)
            public double samprate; // Nominal sample rate (Hz)
            public long samplecnt; // Number of samples in record
            public sbyte encoding; // Data encoding format
            public sbyte byteorder; // Original/Final byte order of record

            /* Data sample fields */
            public object datasamples; // Data samples, 'numsamples' of type 'sampletype'
            public long numsamples; // Number of data samples in datasamples
            public char sampletype; // Sample type code: a, i, f, d

            /* Stream oriented state information */
            public StreamState ststate; // Stream processing state information
            
        }

       public class MSRecord : MSRecord_s { }

        /* Blockette chain link, generic linkable blockette index */
        public class blkt_link_s
        {
            public ushort blktoffset; // Offset to this blockette
            public ushort blkt_type; // Blockette type
            public ushort next_blkt; // Offset to next blockette
            public object blktdata; // Blockette data
            public ushort blktdatalen; // Length of blockette data in bytes
            public blkt_link_s next;
        }

        public class BlktLink : blkt_link_s { }


        /* Blockette 100, Sample Rate (without header) */
        public class blkt_100_s
        {
            public float samprate;
            public sbyte flags;
            public byte[] reserved = new byte[3];
        }

        /* Blockette 1000, Data Only SEED (without header) */
        public class blkt_1000_s
        {
            public byte encoding;
            public byte byteorder;
            public byte reclen;
            public byte reserved;
        }

        /* Blockette 1001, Data Extension (without header) */
        public class blkt_1001_s
        {
            public byte timing_qual;
            public sbyte usec;
            public byte reserved;
            public byte framecnt;
        }

        public class StreamState_s
        {
            public long packedrecords; // Count of packed records
            public long packedsamples; // Count of packed samples
            public int lastintsample; // Value of last integer sample packed
            //public flag comphistory = new flag(); // Control use of lastintsample for compression history
            public bool comphistory = false; // Control use of lastintsample for compression history
        }

        public class StreamState : StreamState_s { }


        /* Leap second declarations, implementation in gentutils.c */
        public class LeapSecond_s
        {
            public hptime_t leapsecond = new hptime_t();
            public int TAIdelta;
            public LeapSecond_s next;
        }

        public class LeapSecond : LeapSecond_s { }
        public static LeapSecond leapsecondlist;





        /* Logging parameters */
        public const int MAX_LOG_MSG_LENGTH = 200; // Maximum length of log messages
        public class MSLogParam_s
        {
            public delegate void log_printDelegate(ref string UnnamedParameter);
            public log_printDelegate log_print;
            public readonly string logprefix;
            public delegate void diag_printDelegate(ref string UnnamedParameter);
            public diag_printDelegate diag_print;
            public readonly string errprefix;
        }

        public class MSLogParam : MSLogParam_s { };
    }



}
