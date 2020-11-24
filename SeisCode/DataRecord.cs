﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SeisCode
{
	/// <summary>
	/// DataRecord.java
	/// 
	/// 
	/// Created: Thu Apr 8 13:52:27 1999
	/// 
	/// @author Philip Crotwell
	/// @version
	/// </summary>

	using Codec = edu.iris.dmc.seedcodec.Codec;
	using CodecException = edu.iris.dmc.seedcodec.CodecException;
	using DecompressedData = edu.iris.dmc.seedcodec.DecompressedData;
	using UnsupportedCompressionType = edu.iris.dmc.seedcodec.UnsupportedCompressionType;

	[Serializable]
	public class DataRecord : SeedRecord
	{

		public DataRecord(DataHeader header) : base(header)
		{
		}

		public DataRecord(DataRecord record) : base(new DataHeader(record.Header.SequenceNum, record.Header.TypeCode, record.Header.Continuation))
		{
			RECORD_SIZE = record.RECORD_SIZE;
			Header.ActivityFlags = record.Header.ActivityFlags;
			Header.ChannelIdentifier = record.Header.ChannelIdentifier;
			Header.DataBlocketteOffset = (short)record.Header.DataBlocketteOffset;
			Header.DataOffset = (short)record.Header.DataOffset;
			Header.DataQualityFlags = record.Header.DataQualityFlags;
			Header.IOClockFlags = record.Header.IOClockFlags;
			Header.LocationIdentifier = record.Header.LocationIdentifier;
			Header.NetworkCode = record.Header.NetworkCode;
			Header.NumSamples = (short)record.Header.NumSamples;
			Header.SampleRateFactor = (short)record.Header.SampleRateFactor;
			Header.SampleRateMultiplier = (short)record.Header.SampleRateMultiplier;
			Header.StartBtime = record.Header.StartBtime;
			Header.StationIdentifier = record.Header.StationIdentifier;
			Header.TimeCorrection = record.Header.TimeCorrection;
			try
			{
				Data = record.Data;
				for (int j = 0; j < record.Blockettes.length; j++)
				{
					blockettes.add(record.Blockettes[j]);
				}
			}
			catch (SeedFormatException e)
			{
				throw new Exception("Shouldn't happen as record was valid and we are copying it", e);
			}
		}

		/// <summary>
		/// Adds a blockette to the record. The number of blockettes in the header is
		/// incremented automatically.
		/// </summary>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void addBlockette(Blockette b) throws SeedFormatException
		public virtual void addBlockette(Blockette b)
		{
			if (b == null)
			{
				throw new System.ArgumentException("Blockette cannot be null");
			}
			if (b is BlocketteUnknown)
			{
				b = new DataBlocketteUnknown(((BlocketteUnknown)b).info, b.Type, ((BlocketteUnknown)b).SwapBytes);
			}
			if (b is DataBlockette)
			{
				base.addBlockette(b);
				Header.NumBlockettes = (sbyte)(Header.NumBlockettes + 1);
			}
			else
			{
				throw new SeedFormatException("Cannot add non-data blockettes to a DataRecord " + b.Type);
			}
			if (b is Blockette1000)
			{
				RecordSize = ((Blockette1000)b).LogicalRecordLength;
			}
			recheckDataOffset();
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: protected void recheckDataOffset() throws SeedFormatException
		protected internal virtual void recheckDataOffset()
		{
			int size = Header.Size;
			Blockette[] blocks = Blockettes;
			for (int i = 0; i < blocks.Length; i++)
			{
				size += blocks[i].Size;
			}
			if (data != null)
			{
				size += data.Length;
			}
			if (size > RECORD_SIZE)
			{
				int headerSize = size;
				if (data != null)
				{
					headerSize = size - data.Length;
				}
				throw new SeedFormatException("Can't fit blockettes and data in record " + headerSize + " + " + (data == null ? 0 : data.Length) + " > " + RECORD_SIZE);
			}
			if (data != null)
			{
				// shift the data to end of blockette so pad happens between
				// blockettes and data
				Header.DataOffset = (short)(RECORD_SIZE - data.Length);
			}
		}

		/// <summary>
		/// returns the data from this data header unparsed, as a byte array in
		/// the format from blockette 1000. The return type is byte[], so the caller
		/// must decode the data based on its format.
		/// </summary>
		public virtual sbyte[] Data
		{
			get
			{
				return data;
			}
			set
			{
				this.data = value;
				recheckDataOffset();
			}
		}

		/// <summary>
		/// Decompress the data in this record according to the compression type in
		/// the header.
		/// 
		/// @return </summary>
		/// <exception cref="SeedFormatException"> if no blockette 1000 present </exception>
		/// <exception cref="UnsupportedCompressionType"> </exception>
		/// <exception cref="CodecException"> </exception>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public edu.iris.dmc.seedcodec.DecompressedData decompress() throws SeedFormatException, edu.iris.dmc.seedcodec.UnsupportedCompressionType, edu.iris.dmc.seedcodec.CodecException
		public virtual DecompressedData decompress()
		{
			// in case of record with only blockettes, ex detection blockette, which often have compression type
			// set to 0, which messes up the decompresser even though it doesn't matter since there is no data.
			if (Header.NumSamples == 0)
			{
				return new DecompressedData(new int[0]);
			}
			Blockette1000 b1000 = (Blockette1000)getUniqueBlockette(1000);
			if (b1000 == null)
			{
				throw new MissingBlockette1000(Header);
			}
			Codec codec = new Codec();
			return codec.decompress(b1000.EncodingFormat, Data, Header.NumSamples, b1000.LittleEndian);
		}


		public virtual int DataSize
		{
			get
			{
				return data.Length;
			}
		}


		public virtual float SampleRate
		{
			get
			{
				float sampleRate;
				Blockette[] blocketts = getBlockettes(100);
				if (blocketts.Length != 0)
				{
					Blockette100 b100 = (Blockette100)blocketts[0];
					sampleRate = b100.ActualSampleRate;
				}
				else
				{
					sampleRate = Header.calcSampleRateFromMultipilerFactor();
				}
				return sampleRate;
			}
		}

		/// <summary>
		/// return a Btime structure containing the derived end time for this record
		/// Note this is not the time of the last sample, but rather the predicted
		/// begin time of the next record, ie begin + numSample*period instead of
		/// begin + (numSample-1)*period.
		/// 
		/// Note that this will use the more accurate sample rate in a blockette100 if it exists.
		/// </summary>
		private Btime EndBtime
		{
			get
			{
				Btime startBtime = Header.StartBtime;
				// get the number of ten thousandths of seconds of data
				double numTenThousandths = (((double)Header.NumSamples / SampleRate) * 10000.0);
				// return the time structure projected by the number of ten thousandths
				// of seconds
				return Header.projectTime(startBtime, numTenThousandths);
			}
		}

		/// <summary>
		/// returns the predicted start time of the next record, ie begin + numSample*period
		/// 
		/// Note that this will use the more accurate sample rate in a blockette100 if it exists.
		/// </summary>
		public virtual Btime PredictedNextStartBtime
		{
			get
			{
				return EndBtime;
			}
		}

		public virtual BtimeRange BtimeRange
		{
			get
			{
				return new BtimeRange(Header.StartBtime, LastSampleBtime);
			}
		}

		/// <summary>
		/// return a Btime structure containing the derived last sample time for this
		/// record.
		/// 
		/// Note that this will use the more accurate sample rate in a blockette100 if it exists.
		/// </summary>
		public virtual Btime LastSampleBtime
		{
			get
			{
				Btime startBtime = StartBtime;
				// get the number of ten thousandths of seconds of data
				double numTenThousandths = (((double)(Header.NumSamples - 1) / SampleRate) * 10000.0);
				// return the time structure projected by the number of ten thousandths
				// of seconds
				return DataHeader.projectTime(startBtime, numTenThousandths);
			}
		}

		/// <summary>
		/// Gets start Btime from header, convenience method. </summary>
		public virtual Btime StartBtime
		{
			get
			{
				return Header.StartBtime;
			}
		}

		/// <summary>
		/// Gets start time from header, convenience method. </summary>
		public virtual string StartTime
		{
			get
			{
				return Header.StartTime;
			}
		}


		/// <summary>
		/// get the value of end time. derived from Start time, sample rate, and
		/// number of samples. Note this is not the time of the last sample, but
		/// rather the predicted begin time of the next record.
		/// 
		/// Note that this will use the more accurate sample rate in a blockette100 if it exists.
		/// </summary>
		/// <returns> the value of end time </returns>
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
		/// Note that this will use the more accurate sample rate in a blockette100 if it exists.
		/// </summary>
		/// <returns> the value of end time </returns>
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

		public virtual DataHeader Header
		{
			get
			{
				return (DataHeader)header;
			}
		}

		public virtual sbyte[] toByteArray()
		{
			try
			{
				MemoryStream byteStream = new MemoryStream();
				DataOutputStream dos = new DataOutputStream(byteStream);
				write(dos);
				dos.close();
				return byteStream.toByteArray();
			}
			catch (IOException e)
			{
				// shouldn't happen
				throw new Exception("Caught IOException, should not happen.", e);
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void write(DataOutputStream dos) throws IOException
		public virtual void write(DataOutputStream dos)
		{
			Blockette[] blocks = Blockettes;
			Header.NumBlockettes = (sbyte)blocks.Length;
			if (blocks.Length != 0)
			{
				Header.DataBlocketteOffset = (sbyte)48;
			}
			Header.write(dos);
			DataBlockette dataB;
			short blockettesSize = Header.Size;
			for (int i = 0; i < blocks.Length; i++)
			{
				dataB = (DataBlockette)blocks[i];
				blockettesSize += (short)dataB.Size;
				if (i != blocks.Length - 1)
				{
					dos.write(dataB.toBytes(blockettesSize));
				}
				else
				{
					dos.write(dataB.toBytes((short)0));
				}
			} // end of for ()
			for (int i = blockettesSize; i < Header.DataOffset; i++)
			{
				dos.write(ZERO_BYTE);
			}
			dos.write(data);
			int remainBytes = RECORD_SIZE - Header.DataOffset - data.Length;
			for (int i = 0; i < remainBytes; i++)
			{
				dos.write(ZERO_BYTE);
			} // end of for ()
		}

		/// @deprecated Confusing method name, use printData(PrintWriter) for textual
		///             output and write(DataOutputStream) for binary output.
		/// 
		/// <param name="out"> </param>
		[Obsolete("Confusing method name, use printData(PrintWriter) for textual")]
		public virtual void writeData(PrintWriter @out)
		{
			printData(@out);
		}

		public virtual void printData(PrintWriter @out)
		{
			sbyte[] d = Data;
			DecimalFormat byteFormat = new DecimalFormat("000");
			int i;
			for (i = 0; i < d.Length; i++)
			{
				@out.write(byteFormat.format(0xff & d[i]) + " ");
				if (i % 4 == 3)
				{
					@out.write("  ");
				}
				if (i % 16 == 15 && i != 0)
				{
					@out.write("\n");
				}
			}
			if (i % 16 != 15 && i != 0)
			{
				@out.write("\n");
			}
		}
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public static SeedRecord readDataRecord(DataInput inStream, DataHeader header, int defaultRecordSize) throws IOException, SeedFormatException
		public static SeedRecord readDataRecord(DataInput inStream, DataHeader header, int defaultRecordSize)
		{
			try
			{
				bool swapBytes = header.flagByteSwap();
				/*
				 * Assert.isTrue(header.getDataBlocketteOffset()>= header.getSize(),
				 * "Offset to first blockette must be larger than the header size");
				 */
				DataRecord dataRec = new DataRecord(header);
				// read garbage between header and blockettes
				if (header.DataBlocketteOffset != 0)
				{
					sbyte[] garbage = new sbyte[header.DataBlocketteOffset - header.Size];
					if (garbage.Length != 0)
					{
						inStream.readFully(garbage);
					}
				}
				sbyte[] blocketteBytes;
				int currOffset = header.DataBlocketteOffset;
				if (header.DataBlocketteOffset == 0)
				{
					currOffset = header.Size;
				}
				int type, nextOffset;
				int recordSize = 0;
				for (int i = 0; i < header.NumBlockettes; i++)
				{
					// get blockette type (first 2 bytes)
					sbyte hibyteType = inStream.readByte();
					sbyte lowbyteType = inStream.readByte();
					type = Utility.uBytesToInt(hibyteType, lowbyteType, swapBytes);
					sbyte hibyteOffset = inStream.readByte();
					sbyte lowbyteOffset = inStream.readByte();
					nextOffset = Utility.uBytesToInt(hibyteOffset, lowbyteOffset, swapBytes);
					// account for the 4 bytes above
					currOffset += 4;
					if (nextOffset != 0)
					{
						blocketteBytes = new sbyte[nextOffset - currOffset];
					}
					else if (header.NumSamples != 0 && header.DataOffset > currOffset)
					{
						blocketteBytes = new sbyte[header.DataOffset - currOffset];
					}
					else if (header.NumSamples == 0 && i == header.NumBlockettes - 1 && recordSize > 0)
					{
						// weird case where no data, only blockettes and so try to load all bytes as the last
						// blockette and trim to fit after reading
						blocketteBytes = new sbyte[recordSize - currOffset];
					}
					else
					{
						blocketteBytes = new sbyte[0];
					}
					inStream.readFully(blocketteBytes);
					if (nextOffset != 0)
					{
						currOffset = nextOffset;
					}
					else
					{
						currOffset += blocketteBytes.Length;
					}
					// fix so blockette has full bytes
					sbyte[] fullBlocketteBytes = new sbyte[blocketteBytes.Length + 4];
					Array.Copy(blocketteBytes, 0, fullBlocketteBytes, 4, blocketteBytes.Length);
					fullBlocketteBytes[0] = hibyteType;
					fullBlocketteBytes[1] = lowbyteType;
					fullBlocketteBytes[2] = hibyteOffset;
					fullBlocketteBytes[3] = lowbyteOffset;

					Blockette b = SeedRecord.BlocketteFactory.parseBlockette(type, fullBlocketteBytes, swapBytes);
					if (b.Type == 1000)
					{
						// might need this in the case of b2000 as its length is dynamic
						// and might be no data so data offset is not useful
						recordSize = ((Blockette1000)b).DataRecordLength;
					}
					dataRec.blockettes.add(b);
					if (nextOffset == 0)
					{
						break;
					}
				}
				try
				{
					recordSize = ((Blockette1000)dataRec.getUniqueBlockette(1000)).DataRecordLength;
				}
				catch (MissingBlockette1000 e)
				{
					if (defaultRecordSize == 0)
					{
						// no default
						throw e;
					}
					// otherwise use default
					recordSize = defaultRecordSize;
				}
				dataRec.RECORD_SIZE = recordSize;
				// read garbage between blockettes and data
				if (header.DataOffset != 0)
				{
					sbyte[] garbage = new sbyte[header.DataOffset - currOffset];
					if (garbage.Length != 0)
					{
						inStream.readFully(garbage);
					}
				}
				sbyte[] timeseries;
				if (header.DataOffset == 0)
				{
					// data record with no data, so gobble up the rest of the record
					timeseries = new sbyte[recordSize - currOffset];
				}
				else
				{
					if (recordSize < header.DataOffset)
					{
						throw new SeedFormatException("recordSize < header.getDataOffset(): " + recordSize + " < " + header.DataOffset);
					}
					timeseries = new sbyte[recordSize - header.DataOffset];
				}
				inStream.readFully(timeseries);
				dataRec.Data = timeseries;
				return dataRec;
			}
			catch (SeedFormatException e)
			{
				e.Header = header;
				throw e;
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public void setRecordSize(int recordSize) throws SeedFormatException
		public virtual int RecordSize
		{
			set
			{
				int tmp = RECORD_SIZE;
				RECORD_SIZE = value;
				try
				{
					recheckDataOffset();
				}
				catch (SeedFormatException e)
				{
					RECORD_SIZE = tmp;
					throw e;
				}
			}
		}
		public override string ToString()
		{
			string s = "Data " + base.ToString();
			s += "\n" + data.Length + " bytes of data read.";
			return s;
		}

		protected internal sbyte[] data;

		internal sbyte ZERO_BYTE = 0;
	}