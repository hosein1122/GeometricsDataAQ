using System;

namespace RingServer
{
    public class Lib
    {
    }

    /* Ring packet header structure, data follows header in the ring */
    /* RW tagged values are set when packets are added to the ring */
    public class RingPacket
    {
        /// <summary>
        /// RW: Packet ID
        /// </summary>
        public long PktID { get; set; }
        /// <summary>
        /// RW: Offset in ring
        /// </summary>
        public long Offset { get; set; }
        /// <summary>
        /// RW: Packet creation time
        /// </summary>
        public DateTime PktTime { get; set; }
        /// <summary>
        /// RW: ID of next packet in stream, 0 if none
        /// </summary>
        public long NextInStream { get; set; }

        /// <summary>
        ///  Packet stream ID, NULL terminated
        /// </summary>
        public string StreamID { get; set; }

        /// <summary>
        ///  Packet data start time
        /// </summary>
        public DateTime DataStart { get; set; }

        /// <summary>
        /// Packet data end time
        /// </summary>
        public DateTime DataEnd { get; set; }

        /// <summary>
        ///  Packet data size in bytes
        /// </summary>
        public uint DataSize { get; set; }
    }

    /* Ring stream structure used for the stream index */
    public class RingStream
    {
        /// <summary>
        ///  Packet stream ID
        /// </summary>
        public string StreamID { get; set; }

        /// <summary>
        /// Earliest packet data start time
        /// </summary>
        public DateTime EarliestDataStartTime
        {
            get; set;
        }
        /// <summary>
        /// Earliest packet data end time
        /// </summary>
        public DateTime EarliestDataEndTime { get; set; }

        /// <summary>
        ///  Earliest packet creation time
        /// </summary>
        public DateTime EarliestPacketTime { get; set; }

        /// <summary>
        /// ID of earliest packet
        /// </summary>
        public long EarliestPacketID { get; set; }

        /// <summary>
        ///  Latest packet data start time
        /// </summary>
        public DateTime LatestDataSatrtTime { get; set; }

        /// <summary>
        /// Latest packet data end time
        /// </summary>
        public DateTime LatestDataEndTime { get; set; }

        /// <summary>
        /// Latest packet creation time
        /// </summary>
        public DateTime LatestPacketTime { get; set; }

        /// <summary>
        /// ID of latest packet
        /// </summary>
        public long LatestPacketID { get; set; }
    }

    internal static class DefineConstants
    {

        /* Static ring parameters */
        public const string RING_SIGNATURE = "RING";
        public const int RING_VERSION = 1;
        /* Ring relative positioning values */
        public const int RINGCURRENT = -1;
        public const int RINGEARLIEST = -2;
        public const int RINGLATEST = -3;
        public const int RINGNEXT = -4;
        /* Define a maximum stream ID string length */
        public const int MAXSTREAMID = 60;
    }

}
