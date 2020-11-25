using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeisCode
{
    public class ControlRecord : SeedRecord
    {

        public ControlRecord(ControlHeader header) : base(header) { }

        /// <summary>
        /// Reads the next control record from the stream. If the record continues, ie a
        /// blockette is too big to fit in the record, then the following record will be read
        /// recursively and combined with the current. </summary>
        /// <param name="inStream"> </param>
        /// <param name="header"> </param>
        /// <param name="defaultRecordSize">
        /// @return </param>
        /// <exception cref="IOException"> </exception>
        /// <exception cref="SeedFormatException"> </exception>
        public static ControlRecord ReadControlRecord(BinaryReader inStream, ControlHeader header, int defaultRecordSize)
        {
            ControlRecord controlRec = ReadSingleControlRecord(inStream, header, defaultRecordSize, null);

            if (controlRec.LastPartialBlockette != null)
            {
                ContinuedControlRecord continuationCR = new ContinuedControlRecord(controlRec);
                ControlRecord nextPartialRecord = controlRec;
                while (nextPartialRecord.LastPartialBlockette != null)
                {
                    ControlHeader nextHeader = ControlHeader.Read(inStream);
                    if (nextHeader is DataHeader)
                    {
                        throw new SeedFormatException("Control record continues, but next record is a DataRecord. curr=" + header.ToString() + "  next=" + nextHeader.ToString());
                    }
                    nextPartialRecord = ReadSingleControlRecord(inStream, nextHeader, nextPartialRecord.RecordSize, nextPartialRecord.LastPartialBlockette);
                    continuationCR.AddContinuation(nextPartialRecord);
                }
                controlRec = continuationCR;
            }
            return controlRec;
        }

        public static ControlRecord ReadSingleControlRecord(BinaryReader inStream, ControlHeader header, int defaultRecordSize, PartialBlockette partialBlockette)
        {

            /*
			 * Assert.isTrue(header.getDataBlocketteOffset()>= header.getSize(),
			 * "Offset to first blockette must be larger than the header size");
			 */
            int recordSize = defaultRecordSize;
            ControlRecord controlRec = new ControlRecord(header);
            byte[] readBytes;
            int currOffset = header.Size;
            if (partialBlockette != null && header.Continuation)
            {
                // need to pull remaining bytes of continued blockette
                Blockette b;
                int length = partialBlockette.TotalSize - partialBlockette.SoFarSize;
                if (recordSize == 0 || length + currOffset < recordSize)
                {
                    readBytes = new byte[length];
                }
                else
                {
                    // not enought bytes to fill blockette, continues in next record
                    readBytes = new byte[recordSize - currOffset];
                }
                //inStream.readFully(readBytes);
                readBytes = inStream.ReadBytes(readBytes.Length);
                currOffset += readBytes.Length;
                b = new PartialBlockette(partialBlockette.Type, readBytes, partialBlockette.SwapBytes, partialBlockette.SoFarSize, partialBlockette.TotalSize);
                // assuming here that a record length blockette (5, 8, 10) will not be split across records
                controlRec.AddBlockette(b);

            }

            while (recordSize == 0 || currOffset <= recordSize - 7)
            {
                string typeStr;
                byte[] typeBytes = new byte[3];
                if (recordSize == 0 || currOffset < recordSize - 3)
                {
                    typeBytes = inStream.ReadBytes(typeBytes.Length);
                    typeStr = Convert.ToString(typeBytes);
                    currOffset += typeBytes.Length;
                }
                else
                {
                    typeStr = THREESPACE;
                }
                // New blockette's type and length did not fit in this record. We assume the rest of the record is garbage
                if (typeStr.Equals(THREESPACE))
                {
                    break;
                }

                if (recordSize != 0 && currOffset >= recordSize - 4)
                {
                    throw new SeedFormatException("Blockette type/length section is split across records");
                }

                int type = int.Parse(typeStr.Trim());
                byte[] lengthBytes = new byte[4];
                lengthBytes = inStream.ReadBytes(lengthBytes.Length);
                string lengthStr = Convert.ToString(lengthBytes);
                currOffset += lengthBytes.Length;
                int length = int.Parse(lengthStr.Trim());
                if (recordSize == 0 || length + currOffset - 7 < recordSize)
                {
                    readBytes = new byte[length - 7];
                }
                else
                {
                    // not enough bytes left in record to fill blockette
                    readBytes = new byte[recordSize - currOffset];
                }
                readBytes = inStream.ReadBytes(readBytes.Length);

                currOffset += readBytes.Length;
                byte[] fullBlocketteBytes = new byte[7 + readBytes.Length]; // less than length in case of continuation
                Array.Copy(typeBytes, 0, fullBlocketteBytes, 0, 3);
                Array.Copy(lengthBytes, 0, fullBlocketteBytes, 3, 4);
                Array.Copy(readBytes, 0, fullBlocketteBytes, 7, readBytes.Length);
                Blockette b;
                if (length == fullBlocketteBytes.Length)
                {
                    b = SeedRecord.BlocketteFactory.ParseBlockette(type, fullBlocketteBytes, true);
                }
                else
                {
                    //special case for partial blockette continued in next record
                    b = new PartialBlockette(type, fullBlocketteBytes, true, 0, length);
                }
                if (b is ControlRecordLengthBlockette)
                {
                    recordSize = ((ControlRecordLengthBlockette)b).LogicalRecordLength;
                }

                controlRec.AddBlockette(b);
            }
            if (recordSize == 0)
            {
                if (defaultRecordSize == 0)
                {
                    // no default
                    throw new SeedFormatException("No blockettes 5, 8 or 10 to indicated record size and no default set");
                }
                else
                {
                    // otherwise use default
                    recordSize = defaultRecordSize;
                }
            }
            controlRec.RECORD_SIZE = recordSize;
            // read garbage between blockettes and end
            byte[] garbage = new byte[recordSize - currOffset];
            if (garbage.Length != 0)
            {
                //inStream.readFully(garbage);
                _ = inStream.ReadBytes(garbage.Length);

            }
            return controlRec;
        }

        public override int RecordSize
        {
            get
            {
                return RECORD_SIZE;
            }
            set
            {
                RECORD_SIZE = value;
            }
        }

        public const string THREESPACE = "   ";
    }
}


